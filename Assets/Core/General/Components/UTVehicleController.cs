using GLShared.General.Enums;
using GLShared.General.Interfaces;
using GLShared.General.Models;
using GLShared.General.ScriptableObjects;
using GLShared.General.Signals;
using GLShared.Networking.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public abstract class UTVehicleController : MonoBehaviour, IVehicleController
    {
        public const float CUSTOM_GRAVITY_MAX_VERTICAL_ANGLE = 35f;

        private const float BRAKE_FORCE_OPPOSITE_INPUT_AND_FORCE_MULTIPLIER = 0.1f;
        private const float BRAKE_FORCE_NO_INPUTS_MULTIPLIER = 0.25f;
        private const float CENTER_OF_MASS_GIZMO_RADIUS = 0.2f;

        [Inject(Id = "mainRig")] protected Rigidbody rig;
        [Inject(Id = "mainTerrain")] protected Terrain terrain;
        [Inject] protected readonly SignalBus signalBus;
        [Inject] protected readonly IEnumerable<IVehicleAxle> allAxles;
        [Inject] protected readonly GameParameters gameParameters;
        [Inject] protected readonly IPlayerInputProvider inputProvider;
        [Inject] protected readonly VehicleStatsBase vehicleStats;
        [Inject] protected readonly DiContainer container;
        [Inject] protected readonly PlayerEntity playerEntity;
        [Inject] protected readonly TerrainChecker terrainChecker;
        [Inject] protected readonly GroundManager groundManager;
        [Inject] protected readonly IPlayerInstaller playerInstaller;
        [Inject(Optional = true)] protected readonly ITurretController turretController;

        [SerializeField] protected Transform centerOfMass;
        [SerializeField] protected VehicleType vehicleType = VehicleType.Car;
        [SerializeField] protected AnimationCurve forwardPowerCurve;
        [SerializeField] protected AnimationCurve backwardPowerCurve;
        [SerializeField][Range(0f, 3f)] protected float idlerBumpingMultiplier = 0.75f;
        [SerializeField] protected LayerMask wheelsCollisionDetectionMask;
        [SerializeField] protected bool runPhysics = true;

        [Header("Force apply points")]
        [SerializeField] protected ForceApplyPoint brakesForceApplyPoint = ForceApplyPoint.WheelConstraintUpperPoint;
        [SerializeField] protected ForceApplyPoint accelerationForceApplyPoint = ForceApplyPoint.WheelHitPoint;

        protected bool hasAnyWheels;
        protected bool hasTurret;

        protected float currentSpeed;
        protected float currentTurningSpeed;

        protected float absoluteInputY;
        protected float absoluteInputX;
        protected float signedInputY;

        protected float maxForwardSpeed;
        protected float maxBackwardsSpeed;
        protected float currentMaxForwardSpeed;
        protected float currentMaxBackwardSpeed;
        protected float currentSpeedRatio;

        protected int allWheelsAmount;

        #region Computed variables
        protected bool isBrake;
        protected float inputY;
        
        protected float currentDriveForce;
        protected float maxEngineForwardPower;
        protected float currentMaxSpeedRatio;
        protected float currentLongitudalGrip;

        protected float forwardForce;
        protected float turnForce;

        protected float verticalAngle;
        protected float absVerticalAngle;
        protected float absHorizontalAngle;

        protected bool isUpsideDown = false;
        protected bool isMovingInDirectionOfInput = true;
        protected bool isGrounded;

        protected Vector3 wheelVelocityLocal;
        protected GroundFrictionPair currentFrictionPair;

        protected string currentTerrainLayer;
        protected float currentSideFriction;
        #endregion

        protected IEnumerable<IPhysicsWheel> allGroundedWheels;
        protected IEnumerable<IPhysicsWheel> allWheels;

        public VehicleType VehicleType => vehicleType;
        public IEnumerable<IVehicleAxle> AllAxles => allAxles;
        public GroundFrictionPair CurrentFrictionPair => currentFrictionPair;

        public bool HasAnyWheels => hasAnyWheels;
        public float CurrentSpeed => currentSpeed;
        public float CurrentTurningSpeed => currentTurningSpeed;
        public float CurrentSpeedRatio => currentSpeedRatio;
        public float AbsoluteInputY => absoluteInputY;
        public float AbsoluteInputX => absoluteInputX;
        public float SignedInputY => signedInputY;
        public float MaxForwardSpeed => maxForwardSpeed;
        public float MaxBackwardsSpeed => maxBackwardsSpeed;
        public float HorizontalAngle => absHorizontalAngle;
        public float CurrentSideFriction => currentSideFriction;

        public bool IsUpsideDown => isUpsideDown;
        public bool IsGrounded => isGrounded;
        public bool HasTurret => hasTurret;
        public virtual bool RunPhysics => runPhysics;
        public LayerMask WheelsCollisionDetectionMask => wheelsCollisionDetectionMask;
        public ForceApplyPoint BrakesForceApplyPoint => brakesForceApplyPoint;
        public ForceApplyPoint AccelerationForceApplyPoint => accelerationForceApplyPoint;
        public IEnumerable<IPhysicsWheel> AllWheels => allWheels;

        public float GetCurrentMaxSpeed()
        {
            return absoluteInputY == 0f ? 0f : (signedInputY > 0f ? currentMaxForwardSpeed : currentMaxBackwardSpeed);
        }

        public virtual void Initialize()
        {
            SetupRigidbody();

            if (runPhysics)
            {
                maxForwardSpeed = forwardPowerCurve.keys[^1].time;
                maxBackwardsSpeed = backwardPowerCurve.keys[^1].time;
            }

            hasAnyWheels = allAxles.Any() && allAxles.Where(axle => axle.HasAnyWheelPair && axle.HasAnyWheel).Any();
            allWheels = GetAllWheelsInAllAxles();
            allWheelsAmount = allWheels.Count();
            hasTurret = turretController != null;

            signalBus.Subscribe<PlayerSignals.OnPlayerSpawned>(OnPlayerSpawned);

            maxEngineForwardPower = forwardPowerCurve.keys[0].value;
        }

        public virtual void SetupRigidbody()
        {
            rig.mass = vehicleStats.Mass;
            rig.drag = vehicleStats.Drag;
            rig.angularDrag = vehicleStats.AngularDrag;

            if (centerOfMass != null)
            {
                rig.centerOfMass = centerOfMass.localPosition;
            }
        }

        protected void CalculateVehicleAngles()
        {
            verticalAngle = 90f - Vector3.Angle(Vector3.up, transform.forward);

            absVerticalAngle = Mathf.Abs(verticalAngle);
            absHorizontalAngle = Mathf.Abs(90f - Vector3.Angle(Vector3.up, transform.right));
        }

        protected virtual void FixedUpdate()
        {
            if (!runPhysics)
            {
                return;
            }

            CheckGroundLayer(terrain);
            CalculateVehicleAngles();
            CalculateVehicleMaxVelocity();

            allGroundedWheels = GetGroundedWheelsInAllAxles().ToArray();
            isGrounded = allGroundedWheels.Any();
            isUpsideDown = CheckUpsideDown();
            isMovingInDirectionOfInput = Mathf.Sign(transform.InverseTransformDirection(rig.velocity).z) == Mathf.Sign(inputProvider.Vertical);
        }

        protected virtual void Update()
        {
            if (!runPhysics)
            {
                return;
            }

            if (inputProvider != null)
            {
                isBrake = inputProvider.Brake;
                inputY = inputProvider.RawVertical == 0f ? 0f : inputProvider.Vertical;

                absoluteInputY = inputProvider.AbsoluteVertical;
                absoluteInputX = inputProvider.AbsoluteHorizontal;

                signedInputY = inputProvider.SignedVertical;
            }
        }

        protected void CalculateVehicleMaxVelocity()
        {
            //Calculating movement, depending on which direction we are moving
            currentMaxSpeedRatio = inputProvider.RawVertical > 0 ?
                Mathf.Abs(1f - verticalAngle / CUSTOM_GRAVITY_MAX_VERTICAL_ANGLE) :
                1f - (absVerticalAngle / CUSTOM_GRAVITY_MAX_VERTICAL_ANGLE);

            if (verticalAngle > 0f)
            {
                currentMaxForwardSpeed = currentMaxSpeedRatio * maxForwardSpeed;
                currentMaxBackwardSpeed = (1f + currentMaxSpeedRatio) * maxBackwardsSpeed;
            }
            else
            {
                currentMaxForwardSpeed = (1f + currentMaxSpeedRatio) * maxForwardSpeed;
                currentMaxBackwardSpeed = currentMaxSpeedRatio * maxBackwardsSpeed;
            }
        }

        protected void SetCurrentSpeed()
        {
            currentSpeed = rig.velocity.magnitude * gameParameters.SpeedMultiplier;
            currentTurningSpeed = Mathf.Abs(rig.angularVelocity.y * Mathf.Rad2Deg);

            float maxSpeed = GetCurrentMaxSpeed();
            currentSpeedRatio = maxSpeed != 0f ? currentSpeed / maxSpeed : 0f;
        }

        protected bool CheckUpsideDown()
        {
            return transform.up.y <= 0.2f;
        }

        protected void EvaluateDriveParams()
        {
            if (inputProvider.RawVertical == 0f)
            {
                currentDriveForce = 0f;
            }
            else
            {
                currentDriveForce = inputProvider.RawVertical > 0f ? forwardPowerCurve.Evaluate(currentSpeed) : backwardPowerCurve.Evaluate(currentSpeed);
            }
        }

        protected void Accelerate()
        {
            if (inputProvider.RawVertical == 0f || isBrake)
            {
                return;
            }

            foreach (var axle in allAxles)
            {
                if(!axle.CanDrive || isBrake || currentSpeed > GetCurrentMaxSpeed())
                {
                    continue;
                }

                var groundedWheels = axle.GroundedWheels;

                if (!groundedWheels.Any())
                {
                    continue;
                }

                foreach (var wheel in groundedWheels)
                {
                    if (!wheel.IsIdler)
                    {
                        if (wheel.HitInfo.NormalAndUpAngle <= gameParameters.MaxWheelDetectionAngle)
                        {
                            var acceleratePoint = wheel.ReturnWheelPoint(accelerationForceApplyPoint);

                            wheelVelocityLocal = wheel.Transform.InverseTransformDirection(rig.GetPointVelocity(acceleratePoint) - wheel.GroundVelocity);
                            forwardForce = inputY * currentDriveForce * Mathf.Max(currentMaxSpeedRatio, 0.6f);
                            turnForce = wheelVelocityLocal.x * currentDriveForce;

                            rig.AddForceAtPosition((forwardForce * wheel.Transform.forward), acceleratePoint);

                            if (currentFrictionPair.IsDefaultLayer)
                            {
                                rig.AddForceAtPosition((turnForce * -wheel.Transform.right), acceleratePoint);
                            }
                        }
                    }
                    else
                    {
                        var idler = (UTIdlerWheel) wheel;

                        wheelVelocityLocal = wheel.Transform.InverseTransformDirection(rig.GetPointVelocity(wheel.Transform.position));
                        forwardForce = inputY * maxEngineForwardPower * idlerBumpingMultiplier;

                        if (currentFrictionPair.IsDefaultLayer)
                        {
                            turnForce = wheelVelocityLocal.x * currentDriveForce;
                            rig.AddForceAtPosition((turnForce * -wheel.Transform.right), wheel.Transform.position);
                        }

                        if ((float)idler.IdlerSite == inputProvider.RawVertical)
                        {
                            var dir = idler.IdlerSite == IdlerWheelSite.Forward ? (wheel.Transform.up + wheel.Transform.forward) : -wheel.Transform.up;
                            rig.AddForceAtPosition((forwardForce * dir), wheel.Transform.position);
                        }
                    }
                }
            }
        }

        protected void Brakes()
        {
            if (!allGroundedWheels.Any() 
                || verticalAngle >= CUSTOM_GRAVITY_MAX_VERTICAL_ANGLE
                || absHorizontalAngle >= currentFrictionPair.HorizontalAnglesRange.Max 
                || !currentFrictionPair.IsDefaultLayer)
            {
                return;
            }

            currentLongitudalGrip = isBrake ? 1f : (inputProvider.RawVertical != 0f ?
               (isMovingInDirectionOfInput ? 0f : BRAKE_FORCE_OPPOSITE_INPUT_AND_FORCE_MULTIPLIER)
               : BRAKE_FORCE_NO_INPUTS_MULTIPLIER);


            if (inputProvider.RawVertical == 0f || isBrake || !isMovingInDirectionOfInput)
            {
                float forceMultiplier = isBrake ? 0.2f : 0.7f;

                foreach (var wheel in allGroundedWheels)
                {
                    if (!wheel.IsIdler)
                    {
                        var brakesPoint = wheel.ReturnWheelPoint(brakesForceApplyPoint);

                        var forwardDir = wheel.Transform.forward;
                        var tireVel = rig.GetPointVelocity(brakesPoint) - wheel.GroundVelocity;

                        float steeringVel = Vector3.Dot(forwardDir, tireVel);
                        float desiredVelChange = -steeringVel * currentLongitudalGrip;
                        float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

                        rig.AddForceAtPosition(desiredAccel * (wheel.TireMass * forceMultiplier) * forwardDir, brakesPoint);
                    }
                }
            }
        }

        protected virtual void CustomGravityLogic()
        {
        }

        protected IEnumerable<IPhysicsWheel> GetGroundedWheelsInAllAxles()
        {
            var result = new List<IPhysicsWheel>();

            if (allAxles.Any())
            {
                foreach (var axle in allAxles)
                {
                    if (axle.GroundedWheels.Any())
                    {
                        result.AddRange(axle.GroundedWheels);
                    }
                }
            }

            return result;
        }

        protected IEnumerable<IPhysicsWheel> GetAllWheelsInAllAxles()
        {
            var result = new List<IPhysicsWheel>();

            if (allAxles.Any())
            {
                foreach (var axle in allAxles)
                {
                    if (axle.HasAnyWheelPair && axle.HasAnyWheel)
                    {
                        result.AddRange(axle.AllWheels);
                    }
                }
            }

            return result;
        }

        private void CheckGroundLayer(Terrain terrain)
        {
            var terrainLayerName = terrainChecker.GetLayerName(transform.position, terrain);

            if (currentTerrainLayer != terrainLayerName)
            {
                currentTerrainLayer = terrainLayerName;
                currentFrictionPair = groundManager.GetPair(currentTerrainLayer);

                if (currentFrictionPair != null)
                {
                    currentSideFriction = currentFrictionPair.GetFrictionForAngle(absHorizontalAngle);
                }
                else
                {
                    currentSideFriction = 1f;
                }
            }
        }

        private void OnPlayerSpawned(PlayerSignals.OnPlayerSpawned OnPlayerSpawned)
        {
            if (!playerInstaller.IsPrototypeInstaller)
            {
                if (OnPlayerSpawned.PlayerProperties.Username == playerEntity.Username)
                {
                    gameObject.name = $"({OnPlayerSpawned.PlayerProperties.PlayerVehicleName}) Player '{playerEntity.Username}'";

                    signalBus.Fire(new PlayerSignals.OnPlayerInitialized()
                    {
                        PlayerProperties = playerEntity.Properties,
                        InputProvider = inputProvider,
                        VehicleStats = vehicleStats,
                    });
                }
            }
            else
            {
                gameObject.name = $"({OnPlayerSpawned.PlayerProperties.PlayerVehicleName}) Player '{playerEntity.Username}'";

                signalBus.Fire(new PlayerSignals.OnPlayerInitialized()
                {
                    PlayerProperties = playerEntity.Properties,
                    InputProvider = inputProvider,
                    VehicleStats = vehicleStats,
                });
            }
        }

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            if (rig == null)
            {
                rig = GetComponent<Rigidbody>();
            }

            Gizmos.color = Color.white;
            Gizmos.DrawSphere(rig.worldCenterOfMass, CENTER_OF_MASS_GIZMO_RADIUS);
            #endif
        }
    }
}
