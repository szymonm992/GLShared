using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using GLShared.General.Interfaces;
using GLShared.General.Enums;
using GLShared.General.ScriptableObjects;
using GLShared.General.Utilities;

namespace GLShared.General.Models
{
    public abstract class UTWheelBase : MonoBehaviour, IInitializable, IPhysicsWheel
    {
        protected const float WHEEL_TURN_RATIO = 8f;

        [Inject] protected readonly GameParameters gameParameters;
        [Inject(Id = "mainRig")] protected Rigidbody rig;
        [Inject] protected readonly IVehicleController vehicleController;

        [Header("Settings")]
        [Range(0.1f, 2f)]
        [SerializeField] protected float suspensionTravel = 0.5f;
        [SerializeField] protected float wheelRadius = 0.7f;
        [SerializeField] protected float tireMass = 60f;
        [Range(0, 1f)]
        [SerializeField] protected float forwardTireGripFactor = 1f, sidewaysTireGripFactor = 1f;

        [SerializeField] protected float spring = 20000f;
        [SerializeField] protected float damper = 3000f;
        [Range(-3f, 0)]
        [SerializeField] protected float hardPointOfTire = -0.7f;
        [SerializeField] protected Transform upperConstraintTransform;
        [SerializeField] protected Transform lowerConstraintTransform;


        [SerializeField]
        protected UTWheelDebug debugSettings = new UTWheelDebug()
        {
            DrawGizmos = true,
            DrawOnDisable = false,
            DrawMode = UTDebugMode.All,
            DrawWheelDirection = true,
            DrawShapeGizmo = true,
            DrawSprings = true,
        };

        #region Telemetry/readonly
        protected HitInfo hitInfo = new HitInfo();
        protected bool isGrounded = false;

        protected float previousSuspensionDistance = 0f;
        protected float normalForce = 0f;
        protected float extension = 0f;
        protected float compressionRate = 0f;
        protected float steerAngle = 0f;
        protected float wheelAngle = 0f;

        protected float absGravity;
        protected float finalTravelLength;
        protected float hardPointAbs;

        protected Vector3 suspensionForce;
        protected Vector3 tirePosition;

        protected Rigidbody localRig;
        protected MeshCollider localCollider;

        #endregion

        public Transform Transform => transform;
        public bool IsGrounded => isGrounded;
        public HitInfo HitInfo => hitInfo;
        public float WheelRadius => wheelRadius;
        public float TireMass => tireMass;
        public float ForwardTireGripFactor => forwardTireGripFactor;
        public float SidewaysTireGripFactor => sidewaysTireGripFactor;
        public float CompressionRate => compressionRate;
        public float HardPointAbs => hardPointAbs;
        public Vector3 TireWorldPosition => tirePosition;
        public Vector3 UpperConstraintPoint => upperConstraintTransform.position;
        public Vector3 LowerConstraintPoint => lowerConstraintTransform.position;

        public float SteerAngle
        {
            get => wheelAngle;
            set => this.steerAngle = value;
        }

        public virtual void Initialize()
        {
            localRig = GetComponent<Rigidbody>();
            localCollider = GetComponent<MeshCollider>();

            AssignPrimaryParameters();
            SetIgnoredColliders();
        }

        protected virtual void AssignPrimaryParameters()
        {
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

        protected virtual void SetIgnoredColliders()
        {
            var allColliders = transform.root.GetComponentsInChildren<Collider>();

            if (allColliders.Any() && localCollider)
            {
                allColliders.ForEach(collider => Physics.IgnoreCollision(localCollider, collider, true));
            }
        }

        protected virtual void Update()
        {
            if (wheelAngle != steerAngle)
            {
                wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, Time.deltaTime * WHEEL_TURN_RATIO);
                transform.localRotation = Quaternion.Euler(transform.localRotation.x,
                    transform.localRotation.y + wheelAngle,
                    transform.localRotation.z);
            }
        }

        protected virtual void FixedUpdate()
        {
            Vector3 newPosition = GetTirePosition();

            if (compressionRate == 1 && vehicleController.DoesGravityDamping)
            {
                GravityCounterforce();
            }
            
            tirePosition = Vector3.Lerp(tirePosition, newPosition, Time.deltaTime * Mathf.Max(50f, 100f * vehicleController.CurrentSpeedRatio));

            normalForce = GetSuspensionForce(tirePosition) + tireMass * absGravity;
            suspensionForce = normalForce * transform.up;

            if (!isGrounded)
            {
                return;
            }

            rig.AddForceAtPosition(suspensionForce, tirePosition);
        }

        protected virtual void GravityCounterforce()
        {
            if (rig.velocity.y < -4f)
            {
                rig.AddForce(Vector3.up * Mathf.Min(-rig.velocity.y, 4f), ForceMode.VelocityChange);
            }
        }

        protected virtual Vector3 GetTirePosition()
        {
            if (localRig.SweepTest(-transform.up, out hitInfo.rayHit, finalTravelLength))
            {
                hitInfo.CalculateNormalAndUpDifferenceAngle();
                isGrounded = (hitInfo.NormalAndUpAngle <= gameParameters?.MaxWheelDetectionAngle);
            }
            else
            {
                isGrounded = false;
            }

            Vector3 tirePos = transform.position - (transform.up * finalTravelLength);

            if (isGrounded)
            {
                tirePos = transform.position - (transform.up * hitInfo.Distance);
                extension = Vector3.Distance(upperConstraintTransform.position, tirePos) / suspensionTravel;
                if (hardPointAbs < (tirePosition - transform.position).sqrMagnitude)
                {
                    compressionRate = 1 - extension;
                }
                else
                {
                    compressionRate = 1;
                    extension = 0;
                }
            }
            else
            {
                extension = 1;
                compressionRate = 0;
            }
            return tirePos;
        }

        protected virtual float GetSuspensionForce(Vector3 tirePosition)
        {
            float distance = Vector3.Distance(lowerConstraintTransform.position, tirePosition);
            float springForce = spring * distance;
            float damperForce = damper * ((distance - previousSuspensionDistance) / Time.fixedDeltaTime);
            previousSuspensionDistance = distance;
            return springForce + damperForce;
        }


    }
}
