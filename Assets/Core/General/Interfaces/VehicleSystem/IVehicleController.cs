using GLShared.General.Enums;
using GLShared.General.Models;
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
        float CurrentTurningSpeed { get; }
        float CurrentSpeedRatio { get; }
        float MaxForwardSpeed { get; }
        float MaxBackwardsSpeed { get; }
        float HorizontalAngle { get; }
        float CurrentSideFriction { get; }

        bool IsUpsideDown { get; }
        bool IsGrounded { get; }
        bool HasTurret { get; }

        LayerMask WheelsCollisionDetectionMask { get; }
        ForceApplyPoint BrakesForceApplyPoint { get; }
        ForceApplyPoint AccelerationForceApplyPoint { get; }

        GroundFrictionPair CurrentFrictionPair { get; }

        abstract float GetCurrentMaxSpeed();
        abstract void SetupRigidbody();

        bool RunPhysics { get; }
    }
}
