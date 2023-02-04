using System.Linq;
using GLShared.General.Utilities;
using UnityEditor;
using UnityEngine;
using Zenject;
using GLShared.General.ScriptableObjects;
using GLShared.General.Models;
using GLShared.General.Interfaces;
using GLShared.General.Enums;
using Frontend.Scripts;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace GLShared.General.Components
{
    public class UTWheel : UTPhysicWheelBase, IInitializable, IPhysicsWheel
    {
        [Header("Settings")]
        [SerializeField] protected float spring = 20000f;
        [SerializeField] protected float damper = 3000f;
        [SerializeField] protected float tireMass = 60f;

        [Range(0, 1f)]
        [SerializeField] protected float forwardTireGripFactor = 1f, sidewaysTireGripFactor = 1f;
        
        [SerializeField] protected Transform upperConstraintTransform;
        [SerializeField] protected Transform lowerConstraintTransform;
        [SerializeField] protected Transform notGroundedTransform;

        [Range(-3f, 0)]
        [SerializeField] protected float hardPointOfTire = -0.7f;
        [Range(0.1f, 2f)]
        [SerializeField] protected float suspensionTravel = 0.5f;

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
        protected float previousSuspensionDistance = 0f;
        protected float normalForce = 0f;
        protected float extension = 0f;
        protected float compressionRate = 0f;
        protected float absGravity;
        protected float finalTravelLength;
        protected float hardPointAbs;
        protected Rigidbody localRig;
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

        public override void Initialize()
        {
            base.Initialize();

            localRig = GetComponent<Rigidbody>();
            localCollider = GetComponent<MeshCollider>();

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
                Vector3 highestPoint = transform.position + transform.up * hardPointOfTire;
                upperConstraintTransform.position = highestPoint;
            }

            if (lowerConstraintTransform != null)
            {
                Vector3 lowestPoint = transform.position + transform.up * -finalTravelLength;
                lowerConstraintTransform.position = lowestPoint;
            }

            tirePosition = GetTirePosition();
        }

        protected override void FixedUpdate()
        {
            if(vehicleController == null)
            {
                return;
            }

            base.FixedUpdate();
            Vector3 newPosition = GetTirePosition();

            if (vehicleController.RunPhysics && compressionRate == 1 && vehicleController.DoesGravityDamping)
            {
                GravityCounterforce();
            }

            tirePosition = Vector3.Lerp(tirePosition, newPosition, Time.deltaTime * Mathf.Max(50f, 100f * vehicleController.CurrentSpeedRatio));


            NativeArray<float> result = new(2, Allocator.TempJob);
            GetSuspensionForceJob jobData = new()
            {
                damper = damper,
                spring = spring,
                previousSuspensionDistance = previousSuspensionDistance,
                fixedTime = Time.fixedDeltaTime,
                lowerConstraintPoint = LowerConstraintPoint,
                tirePosition = tirePosition,
                result = result,
            };

            JobHandle handle = jobData.Schedule();
            handle.Complete();

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

            if (isGrounded)
            {
                Vector3 steeringDir = Transform.right;
                Vector3 tireVel = rig.GetPointVelocity(UpperConstraintPoint);

                float steeringVel = Vector3.Dot(steeringDir, tireVel);
                float desiredVelChange = -steeringVel * sidewaysTireGripFactor;
                float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

                rig.AddForceAtPosition(desiredAccel * tireMass * steeringDir, UpperConstraintPoint);
            }
        }

        protected override Vector3 GetTirePosition()
        {
            if (localRig.SweepTest(-transform.up, out hitInfo.rayHit, finalTravelLength))
            {
                bool canCollide = gameParameters != null ? (hitInfo != null ? hitInfo.CanCollide(gameParameters.MaxWheelDetectionAngle, vehicleController.WheelsCollisionDetectionMask) : false) : false;
                isGrounded = canCollide;
            }
            else
            {
                isGrounded = false;
            }

            Vector3 tirePos = NotGroundedWheelPosition;

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

        private void GravityCounterforce()
        {
            if (rig.velocity.y < -4f)
            {
                rig.AddForce(Vector3.up * Mathf.Min(-rig.velocity.y, 4f), ForceMode.VelocityChange);
            }
        }

        [BurstCompile]
        private struct GetSuspensionForceJob : IJob
        {
            [ReadOnly] public Vector3 tirePosition;
            [ReadOnly] public Vector3 lowerConstraintPoint;
            [ReadOnly] public float spring;
            [ReadOnly] public float damper;
            [ReadOnly] public float fixedTime;
            [ReadOnly] public float previousSuspensionDistance;

            public NativeArray<float> result;

            public void Execute()
            {
                float distance = Vector3.Distance(lowerConstraintPoint, tirePosition);
                float springForce = spring * distance;
                float damperForce = damper * ((distance - previousSuspensionDistance) / fixedTime);
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
