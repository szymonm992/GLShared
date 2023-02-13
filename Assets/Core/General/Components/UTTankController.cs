using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public class UTTankController : UTVehicleController
    {
        [Inject] private readonly IEnumerable<UTIdlerWheel> idlerWheels;

        public IEnumerable<UTIdlerWheel> IdlerWheels => idlerWheels;

        protected override void FixedUpdate()
        {
            if (!runPhysics)
            {
                return;
            }

            base.FixedUpdate();
            CustomGravityLogic();

            if (!isUpsideDown)
            {
                EvaluateDriveParams();
                Accelerate();
                Brakes();

                SetCurrentSpeed();
            }
        }

        protected override void CustomGravityLogic()
        {
            if (!allGroundedWheels.Where(wheel => !wheel.IsIdler).Any())
            {
                rig.AddForce(Physics.gravity, ForceMode.Acceleration);
            }
            else
            {
                if (currentFrictionPair.IsDefaultLayer)
                {
                    if (currentFrictionPair.HorizontalAnglesRange.Max >= absHorizontalAngle && CUSTOM_GRAVITY_MAX_VERTICAL_ANGLE >= absVerticalAngle)
                    {
                        //If we dont overreach maximum angle for default ground, we apply normalized gravity
                        rig.AddForce(-transform.up * Physics.gravity.magnitude, ForceMode.Acceleration);
                    }
                    else
                    {
                        //If we overreach maximum angle for default ground, we apply multiplied gravity

                        (float currentOverreachAngle, float maxAllowedAngleInDirection) = absHorizontalAngle > absVerticalAngle ?
                            (absHorizontalAngle, currentFrictionPair.HorizontalAnglesRange.Max) : (absVerticalAngle, CUSTOM_GRAVITY_MAX_VERTICAL_ANGLE);
                        float ratio = (currentOverreachAngle / maxAllowedAngleInDirection);

                        rig.AddForce(Physics.gravity * ratio, ForceMode.Acceleration);
                    }
                }
                else
                {
                    //If the ground is not a default ground then we apply multiplied gravity
                    (float currentOverreachAngle, float maxAllowedAngleInDirection) = absHorizontalAngle > absVerticalAngle ?
                            (absHorizontalAngle, currentFrictionPair.HorizontalAnglesRange.Max) : (absVerticalAngle, CUSTOM_GRAVITY_MAX_VERTICAL_ANGLE);
                    float ratio = (currentOverreachAngle / maxAllowedAngleInDirection);

                    rig.AddForce(Physics.gravity * ratio, ForceMode.Acceleration);
                }

                /*
                if (CUSTOM_GRAVITY_MAX_HORIZONTAL_ANGLE >= absHorizontalAngle && CUSTOM_GRAVITY_MAX_VERTICAL_ANGLE >= absVerticalAngle)
                {
                    ApplyNormalizedGravity();
                }
                else
                {
                    (float currentOverreachAngle, float maxAllowedAngleInDirection) = absHorizontalAngle > absVerticalAngle ?
                        (absHorizontalAngle, CUSTOM_GRAVITY_MAX_HORIZONTAL_ANGLE): (absVerticalAngle, CUSTOM_GRAVITY_MAX_VERTICAL_ANGLE);
                    float ratio = (currentOverreachAngle / maxAllowedAngleInDirection);

                    rig.AddForce(Physics.gravity * ratio, ForceMode.Acceleration);
                }*/
            }
        }
    }
}
