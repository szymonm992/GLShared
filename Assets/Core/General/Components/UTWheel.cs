using UnityEditor;
using UnityEngine;
using Zenject;
using GLShared.General.Models;
using GLShared.General.Interfaces;
using GLShared.General.Enums;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace GLShared.General.Components
{
    public class UTWheel : UTPhysicWheelBase, IInitializable, IPhysicsWheel
    {
        private const float FULLY_COMPRESSED_SPRING_MULTIPLIER = 2f;

        [Header("Settings")]
        [SerializeField] protected float spring = 220000f;
        [SerializeField] protected float damper = 40000f;
        [SerializeField] protected float tireMass = 650f;

        [Range(0, 1f)]
        [SerializeField] protected float forwardTireGripFactor = 1f, sidewaysTireGripFactor = 1f;
        
        [SerializeField] protected Transform upperConstraintTransform;
        [SerializeField] protected Transform lowerConstraintTransform;
        [SerializeField] protected Transform notGroundedTransform;

        [Header("Jump effect")]
        [SerializeField] private float jumpMinimalSpeed = 15f;
        [SerializeField] private float damperOnJumpMultiplier = 0.25f;

        [Range(-3f, 0)]
        [SerializeField] protected float hardPointOfTire = -0.7f;
        [Range(0.1f, 2f)]
        [SerializeField] protected float suspensionTravel = 0.5f;

        [SerializeField] private bool enableFallJump = true;
        [SerializeField] private float ungroundTimeMinThreshold = 0.1f;
        [SerializeField] private float ungroundTimeMaxThreshold = 1f;

        [SerializeField]
        protected UTWheelDebug debugSettings = new()
        {
            DrawGizmos = true,
            DrawOnDisable = false,
            DrawMode = UTDebugMode.All,
            DrawWheelDirection = true,
            DrawShapeGizmo = false,
            DrawSprings = true,
        };

        #region Telemetry/readonly
        protected float previousSuspensionDistance;
        protected float normalForce;
        protected float extension;
        protected float compressionRate;
        protected float absGravity;
        protected float finalTravelLength;
        protected float hardPointAbs;
        protected bool isOnTopOfAnotherVehicle;

        protected Rigidbody localRig;

        private float currentSpring;
        private float currentDamper;
        private float currentUngroundedTime;
        #endregion

        public override float TireMass => tireMass;
        public override float CompressionRate => compressionRate;
        public override float ForwardTireGripFactor
        {
            get => forwardTireGripFactor;
            set => this.forwardTireGripFactor = value;
        }
        public override float SidewaysTireGripFactor
        {
            get => sidewaysTireGripFactor;
            set => this.sidewaysTireGripFactor = value;
        }
        public override float HardPointAbs => hardPointAbs;
        public override Vector3 TireWorldPosition => tirePosition;
        public override Vector3 UpperConstraintPoint
        {
            get
            {
                return upperConstraintTransform != null ? upperConstraintTransform.position : transform.position;
            }
        }
        public override Vector3 LowerConstraintPoint
        {
            get
            {
                return lowerConstraintTransform != null ? lowerConstraintTransform.position : transform.position;
            }
        }
        public override Vector3 NotGroundedWheelPosition
        {
            get
            {
                return notGroundedTransform != null ? notGroundedTransform.position :
                    lowerConstraintTransform != null ? LowerConstraintPoint : Vector3.zero;
            }
        }
        public override Vector3 GroundVelocity
        {
            get
            {
                return IsGrounded && hitInfo.Collider && hitInfo.Collider.attachedRigidbody
                    ? hitInfo.Collider.attachedRigidbody.GetPointVelocity(hitInfo.Point)
                    : Vector3.zero;
            }
        }

        public override bool IsOnTopOfAnotherVehicle => isOnTopOfAnotherVehicle;

        public override void Initialize()
        {
            base.Initialize();

            localRig = GetComponent<Rigidbody>();
            localCollider = GetComponent<MeshCollider>();

            currentSpring = spring;
            currentDamper = damper;

            AssignPrimaryParameters();
            SetIgnoredColliders();
        }

        protected override void AssignPrimaryParameters()
        {
            base.AssignPrimaryParameters();

            if (extension == 0f)
            {
                extension = suspensionTravel;
            }

            absGravity = Mathf.Abs(Physics.gravity.y);
            hardPointAbs = Mathf.Abs(hardPointOfTire);
            finalTravelLength = suspensionTravel + hardPointAbs;

            if (upperConstraintTransform != null)
            {
                //Highest point of wheel.
                upperConstraintTransform.position = transform.position + transform.up * hardPointOfTire;
            }

            if (lowerConstraintTransform != null)
            {
                //Lowest point of wheel.
                lowerConstraintTransform.position = transform.position + transform.up * -finalTravelLength;
            }

            tirePosition = GetTirePosition();
        }

        protected override void FixedUpdate()
        {
            if (vehicleController == null)
            {
                return;
            }

            base.FixedUpdate();

            tirePosition = Vector3.Lerp(tirePosition, GetTirePosition(), Time.deltaTime * 100f); //Mathf.Max(50f, 100f * vehicleController.CurrentSpeedRatio));

            CalculateUngroundedTime();
            NativeArray<float> result = new (2, Allocator.TempJob);

            GetSuspensionForceJob jobData = new ()
            {
                isGrounded = isGrounded,
                isAxleGrounded = Axle.IsAnyWheelInAxleAtContact,

                damper = currentDamper,
                spring = currentSpring,

                previousSuspensionDistance = previousSuspensionDistance,
                fixedTime = Time.fixedDeltaTime,
                lowerConstraintPoint = LowerConstraintPoint,
                tirePosition = tirePosition,

                currentSpeed = vehicleController.CurrentSpeed,
                currentSpeedRatio = vehicleController.CurrentSpeedRatio,
                currentCompression = compressionRate,

                enableFallJump = enableFallJump,
                currentUndegroundTime = currentUngroundedTime,
                jumpMinimalSpeed = jumpMinimalSpeed,
                damperOnJumpMultiplier = damperOnJumpMultiplier,
                ungroundTimeMinThreshold = ungroundTimeMinThreshold,

                result = result,
            };

            jobData.Schedule().Complete();

            normalForce = result[0] + tireMass * absGravity;
            previousSuspensionDistance = result[1];
            result.Dispose();

            suspensionForce = normalForce * (vehicleController.HorizontalAngle >= gameParameters.WheelForceDirectionChangeAngle ? hitInfo.Normal : transform.up);
            
            if (!vehicleController.RunPhysics || !isGrounded)
            {
                return;
            }

            rig.AddForceAtPosition(suspensionForce, tirePosition);
            ApplyFriction();
        }

        protected override void ApplyFriction()
        {
            base.ApplyFriction();

            var lowerRig = hitInfo.Collider.attachedRigidbody;
            

            if (isGrounded)
            {
                var steeringDir = Transform.right;
                var tireVel = rig.GetPointVelocity(Transform.position);

                // Moving platform side friction
                if (lowerRig)
                {
                    tireVel -= lowerRig.GetPointVelocity(Transform.position);
                }

                float steeringVel = Vector3.Dot(steeringDir, tireVel);
                float desiredVelChange = -steeringVel * sidewaysTireGripFactor * vehicleController.CurrentSideFriction;
                float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

                rig.AddForceAtPosition(desiredAccel * tireMass * steeringDir, Transform.position);
            }
            
            // Moving platform forward friction
            if (isGrounded && lowerRig)
            {
                var forwardDir = Vector3.Cross(Transform.right, hitInfo.Normal);

                if(Vector3.Dot(forwardDir, lowerRig.velocity.normalized) < 0.0f)
                {
                    forwardDir *= -1.0f;
                }

                var currentContactForce = rig.GetPointVelocity(hitInfo.Point);
                var lowerRigContactForce = lowerRig.GetPointVelocity(hitInfo.Point);

                var missingVelocity = Vector3.Dot(lowerRigContactForce, forwardDir) - Vector3.Dot(currentContactForce, forwardDir);
                //missingVelocity *= forwardTireGripFactor;

                var force = forwardDir.normalized * Mathf.Max(missingVelocity, 0.0f);
                rig.AddForceAtPosition(force, hitInfo.Point, ForceMode.Acceleration);
            }
        }

        protected override Vector3 GetTirePosition()
        {
            if (localRig.SweepTest(-transform.up, out hitInfo.rayHit, finalTravelLength))
            {
                //We're checking if we can collide with this piece of code
                isGrounded = gameParameters != null && (hitInfo != null && hitInfo.CanCollide(gameParameters.MaxWheelDetectionAngle, vehicleController.WheelsCollisionDetectionMask));
            }
            else
            {
                isGrounded = false;
            }

            var tirePos = NotGroundedWheelPosition;

            if (isGrounded)
            {
                tirePos = transform.position - (transform.up * hitInfo.Distance);
                extension = Vector3.Distance(upperConstraintTransform.position, tirePos) / suspensionTravel;

                if (hardPointAbs < Vector3.Distance(tirePosition, transform.position))
                {
                    compressionRate = 1f - extension;
                }
                else
                {
                    compressionRate = 1f;
                    extension = 0f;
                }
            }
            else
            {
                extension = 1f;
                compressionRate = 0f;
            }

            return tirePos;
        }

        private void CalculateUngroundedTime()
        {
            if (!Axle.IsAnyWheelInAxleAtContact && vehicleController.CurrentSpeed > 1f && currentUngroundedTime <= ungroundTimeMaxThreshold)//1 is a threshold value
            {
                currentUngroundedTime += Time.deltaTime;
            }
            else
            {
                if (currentUngroundedTime > 0f)
                {
                    currentUngroundedTime -= Time.deltaTime;
                }
                else
                {
                    currentUngroundedTime = 0f;
                }
            }
        }

        [BurstCompile]
        private struct GetSuspensionForceJob : IJob
        {
            [ReadOnly] public Vector3 tirePosition;
            [ReadOnly] public Vector3 lowerConstraintPoint;
            [ReadOnly] public bool isGrounded;
            [ReadOnly] public bool isAxleGrounded;

            [ReadOnly] public bool enableFallJump;
            [ReadOnly] public float spring;
            [ReadOnly] public float damper;
            [ReadOnly] public float fixedTime;
            [ReadOnly] public float currentCompression;

            [ReadOnly] public float currentUndegroundTime;
            [ReadOnly] public float ungroundTimeMinThreshold;
            [ReadOnly] public float damperOnJumpMultiplier;
            [ReadOnly] public float jumpMinimalSpeed;

            [ReadOnly] public float currentSpeed;
            [ReadOnly] public float currentSpeedRatio;
            [ReadOnly] public float previousSuspensionDistance;

            public NativeArray<float> result;

            public void Execute()
            {
                float currentSpring = spring;
                float currentDamper = damper;

                if (enableFallJump && isGrounded && currentSpeed >= jumpMinimalSpeed && currentUndegroundTime >= ungroundTimeMinThreshold)
                {
                    currentSpring *= (1f + (currentUndegroundTime * Mathf.Min(1f, Mathf.Max(currentSpeedRatio, 0f))));
                    currentDamper *= damperOnJumpMultiplier;
                }

                float distance = Vector3.Distance(lowerConstraintPoint, tirePosition);
                float springForce = currentSpring * distance;
                float damperForce = currentDamper * ((distance - previousSuspensionDistance) / fixedTime);

                result[0] = springForce + damperForce;
                result[1] = distance;
            }
        }

        #if UNITY_EDITOR
        #region DEBUG
        private void OnValidate()
        {
            if (rig == null)
            {
                rig = transform.GetComponentInParent<Rigidbody>();
                localRig = GetComponent<Rigidbody>();
                localCollider = GetComponent<MeshCollider>();
            }

            AssignPrimaryParameters();
            SetIgnoredColliders();
        }
        private void OnDrawGizmos()
        {

            bool drawCurrently = (debugSettings.DrawGizmos) && (debugSettings.DrawMode == UTDebugMode.All)
                || (debugSettings.DrawMode == UTDebugMode.EditorOnly && !Application.isPlaying)
                || (debugSettings.DrawMode == UTDebugMode.PlaymodeOnly && Application.isPlaying);

            if (drawCurrently && (this.enabled) || (debugSettings.DrawOnDisable && !this.enabled))
            {
                if (rig != null)
                {
                    if (!Application.isPlaying)
                    {
                        tirePosition = GetTirePosition();
                    }

                    if (debugSettings.DrawSprings)
                    {
                        Handles.DrawDottedLine(transform.position, tirePosition, 1.1f);


                        if (upperConstraintTransform != null)
                        {
                            Handles.color = Color.white;
                            Handles.DrawLine(upperConstraintTransform.position + transform.forward * 0.1f,
                                upperConstraintTransform.position - transform.forward * 0.1f, 2f);
                        }

                        if (lowerConstraintTransform != null)
                        {
                            Handles.color = Color.white;
                            Handles.DrawLine(lowerConstraintTransform.position + transform.forward * 0.1f, 
                                lowerConstraintTransform.position - transform.forward * 0.1f, 2f);
                        }

                        if(notGroundedTransform != null)
                        {
                            Gizmos.color = Color.red;
                            Handles.DrawWireCube(notGroundedTransform.position, new(0.06f, 0.06f, 0.06f));
                        }

                        Handles.color = Color.white;
                        Handles.DrawLine(tirePosition + transform.forward * 0.05f, tirePosition - transform.forward * 0.05f, 4f);
                    }

                    if (isGrounded)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(hitInfo.Point, .08f);
                    }

                    if (debugSettings.DrawWheelDirection)
                    {
                        Handles.color = isGrounded ? Color.green : Color.red;
                        Handles.DrawLine(tirePosition, tirePosition + transform.forward, 2f);
                    }

                    if (debugSettings.DrawShapeGizmo)
                    {
                        Gizmos.color = isGrounded ? Color.green : Color.red;
                        //Gizmos.DrawWireSphere(tirePosition, wheelRadius);

                        Gizmos.DrawWireMesh(localCollider.sharedMesh, tirePosition, transform.rotation, transform.lossyScale);
                    }
                }
            }
        }

        #endregion
        #endif
    }
}
