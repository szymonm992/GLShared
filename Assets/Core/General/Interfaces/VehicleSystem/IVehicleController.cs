using GLShared.General.Enums;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GLShared.General.Interfaces
{
    public interface IVehicleController : IInitializable
    {
        VehicleType VehicleType { get; }
        bool HasAnyWheels { get; }
        bool DoesGravityDamping { get; }
        IEnumerable<IVehicleAxle> AllAxles { get; }
        IEnumerable<IPhysicsWheel> AllWheels { get; }

        float AbsoluteInputY { get; }
        float AbsoluteInputX { get; }
        float SignedInputY { get; }

        float CurrentSpeed { get; }
        float CurrentSpeedRatio { get; }
        float MaxForwardSpeed { get; }
        float MaxBackwardsSpeed { get; }
        bool IsUpsideDown { get; }
        bool HasTurret { get; }
        LayerMask WheelsCollisionDetectionMask { get; }
        ForceApplyPoint BrakesForceApplyPoint { get; }
        ForceApplyPoint AccelerationForceApplyPoint { get; }
        float VisualElementsMovementSpeed { get; }

        abstract float GetCurrentMaxSpeed();
        abstract void SetupRigidbody();
    }
}
