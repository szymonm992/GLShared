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
    public abstract class UTPhysicWheelBase : MonoBehaviour, IPhysicsWheel
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
        [SerializeField] protected Transform notGroundedTransform;


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
        public Vector3 UpperConstraintPoint
        {
            get
            {
                return upperConstraintTransform != null ? upperConstraintTransform.position : transform.position;
            }
        }
        public Vector3 LowerConstraintPoint
        {
            get
            {
                return lowerConstraintTransform != null ? lowerConstraintTransform.position : transform.position;
            }
        }

        public Vector3 NotGroundedWheelPosition 
        {
            get
            {
                return notGroundedTransform != null ? notGroundedTransform.position : 
                    lowerConstraintTransform != null ? LowerConstraintPoint : Vector3.zero;
            }
        }

        public float SteerAngle
        {
            get => wheelAngle;
            set => this.steerAngle = value;
        }

        public virtual void Initialize()
        {
            
        }

        protected virtual void AssignPrimaryParameters()
        {
            
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
            
        }

        protected virtual void ApplyFriction()
        {

        }

        
        protected virtual Vector3 GetTirePosition()
        {
            return Vector3.zero;
        }

        
    }
}
