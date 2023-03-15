using UnityEngine;
using Zenject;
using System.Linq;
using GLShared.General.Interfaces;
using GLShared.General.ScriptableObjects;
using GLShared.General.Utilities;
using GLShared.General.Components;

namespace GLShared.General.Models
{
    public abstract class UTPhysicWheelBase : MonoBehaviour, IPhysicsWheel
    {
        protected const float WHEEL_TURN_RATIO = 8f;

        [Inject] protected readonly GameParameters gameParameters;
        [Inject(Id = "mainRig")] protected Rigidbody rig;
        [Inject] protected readonly IVehicleController vehicleController;

        [SerializeField] protected float wheelRadius = 0.7f;
        
        #region Telemetry/readonly
        protected bool isGrounded = false;
        protected float steerAngle = 0f;
        protected float wheelAngle = 0f;
        protected Vector3 tirePosition;
        protected Vector3 suspensionForce;
        protected MeshCollider localCollider;
        protected HitInfo hitInfo = new HitInfo();
        #endregion

        public Transform Transform => transform;
        public bool IsGrounded => isGrounded;
        public float WheelRadius => wheelRadius;

        public virtual HitInfo HitInfo => hitInfo;
        public virtual float TireMass => 0f;
        public virtual float ForwardTireGripFactor { get; set; }
        public virtual float SidewaysTireGripFactor { get; set; }
        public virtual float CompressionRate => 0f;
        public virtual float HardPointAbs => 0f;
        public virtual Vector3 TireWorldPosition => Vector3.zero;
        public virtual bool IsIdler => false;
        public virtual bool IsOnTopOfAnotherVehicle => false;

        public virtual Vector3 UpperConstraintPoint
        {
            get
            {
                return transform.position;
            }
        }
        public virtual Vector3 LowerConstraintPoint
        {
            get
            {
                return transform.position;
            }
        }

        public virtual Vector3 NotGroundedWheelPosition 
        {
            get
            {
                return Vector3.zero;
            }
        }

        public virtual Vector3 GroundVelocity
        {
            get
            {
                return Vector3.zero;
            }
        }

        public float SteerAngle
        {
            get => wheelAngle;
            set => this.steerAngle = value;
        }
        public IVehicleAxle Axle { get; set; }

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
