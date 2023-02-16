using Frontend.Scripts.Interfaces;
using GLShared.General.Interfaces;
using GLShared.General.Models;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public class UTTankSteering : MonoBehaviour, IVehicleSteering
    {
        public const float OVERREACH_MAX_ANGLE_STEER_FORCE = 0.1f;
        public const float IDLER_TURN_FORCE_MULTIPLIER = 0.3f;

        [Inject(Id = "mainRig")] private readonly Rigidbody rig;
        [Inject] private readonly IVehicleController suspensionController;
        [Inject] private readonly IPlayerInputProvider inputProvider;
        [Inject] private readonly VehicleStatsBase vehicleStats;

        private float diagonalTurnForceMultiplier;
        private float steerInput;
        private float currentSteerForce;

        public void Initialize()
        {
            diagonalTurnForceMultiplier = (1.0f / Mathf.Sqrt(2));
        }

        public void SetSteeringInput(float input)
        {
            steerInput = inputProvider.AbsoluteVertical != 0 ? input * inputProvider.SignedVertical : input;
        }

        private void Update()
        {
            if (suspensionController != null)
            {
                SetSteeringInput(inputProvider.Horizontal);
            }
        }

        private void FixedUpdate()
        {
            if (steerInput == 0 || suspensionController.IsUpsideDown)
            {
                return;
            }

            currentSteerForce = GetCurrentSteerForce();

            if (inputProvider.CombinedInput > 1f)
            {
                currentSteerForce *= diagonalTurnForceMultiplier;
            }

            if (!suspensionController.RunPhysics)
            {
                return;
            }

            foreach (var axle in suspensionController.AllAxles)
            {
                if (axle.CanSteer)
                {
                    var wheelsInAxle = axle.AllWheels;

                    foreach (var wheel in wheelsInAxle)
                    {
                        int invertValue = axle.InvertSteer ? -1 : 1;

                        if (wheel.IsGrounded)
                        {
                            if (wheel.IsIdler)
                            {
                                currentSteerForce *= IDLER_TURN_FORCE_MULTIPLIER;
                            }

                            rig.AddForceAtPosition(invertValue * currentSteerForce * steerInput * rig.transform.right, wheel.HitInfo.Point, ForceMode.Force);
                        }
                    }
                }
            }
        }

        private float GetCurrentSteerForce()
        {
            var currentPair = suspensionController.CurrentFrictionPair;

            if (currentPair == null)
            {
                return vehicleStats.SteerForce;
            }

            if (suspensionController.HorizontalAngle >= currentPair.HorizontalAnglesRange.Max)
            {
                return OVERREACH_MAX_ANGLE_STEER_FORCE;
            }

            return currentPair.IsDefaultLayer ? vehicleStats.SteerForce : vehicleStats.SteerForce * currentPair.SteeringMultiplier;
        }
    }
}
