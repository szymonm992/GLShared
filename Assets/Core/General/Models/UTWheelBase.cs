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
        private const float WHEEL_TURN_RATIO = 8f;

        [Inject] private readonly GameParameters gameParameters;
        [Inject(Id = "mainRig")] private Rigidbody rig;
        //[Inject] private readonly IVehicleController vehicleController;

        [Header("Settings")]
        [Range(0.1f, 2f)]
        [SerializeField] private float suspensionTravel = 0.5f;
        [SerializeField] private float wheelRadius = 0.7f;
        [SerializeField] private float tireMass = 60f;
        [Range(0, 1f)]
        [SerializeField] private float forwardTireGripFactor = 1f, sidewaysTireGripFactor = 1f;

        [SerializeField] private float spring = 20000f;
        [SerializeField] private float damper = 3000f;
        [Range(-3f, 0)]
        [SerializeField] private float hardPointOfTire = -0.7f;
        [SerializeField] private Transform upperConstraintTransform;
        [SerializeField] private Transform lowerConstraintTransform;


        [SerializeField]
        private UTWheelDebug debugSettings = new UTWheelDebug()
        {
            DrawGizmos = true,
            DrawOnDisable = false,
            DrawMode = UTDebugMode.All,
            DrawWheelDirection = true,
            DrawShapeGizmo = true,
            DrawSprings = true,
        };

        #region Telemetry/readonly
        private HitInfo hitInfo = new HitInfo();
        private bool isGrounded = false;

        private float previousSuspensionDistance = 0f;
        private float normalForce = 0f;
        private float extension = 0f;
        private float compressionRate = 0f;
        private float steerAngle = 0f;
        private float wheelAngle = 0f;

        private float absGravity;
        private float finalTravelLength;
        private float hardPointAbs;

        private Vector3 suspensionForce;
        private Vector3 tirePosition;

        private Rigidbody localRig;
        private MeshCollider localCollider;

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

        public void Initialize()
        {
            localRig = GetComponent<Rigidbody>();
            localCollider = GetComponent<MeshCollider>();

            AssignPrimaryParameters();
            SetIgnoredColliders();
        }

        private void AssignPrimaryParameters()
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
        }

        private void SetIgnoredColliders()
        {
            var allColliders = transform.root.GetComponentsInChildren<Collider>();

            if (allColliders.Any() && localCollider)
            {
                allColliders.ForEach(collider => Physics.IgnoreCollision(localCollider, collider, true));
            }
        }

    }
}
