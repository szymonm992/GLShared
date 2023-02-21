using GLShared.General.Models;
using UnityEngine;
using Zenject;

namespace GLShared.General.Interfaces
{
    public interface IPhysicsWheel : IInitializable
    {
        Transform Transform { get; }
        bool IsGrounded { get; }
        bool IsOnTopOfAnotherVehicle { get; }
        HitInfo HitInfo { get; }
        float WheelRadius { get; }
        float TireMass { get; }
        float ForwardTireGripFactor { get; set; }
        float SidewaysTireGripFactor { get; set; }
        float CompressionRate { get; }
        float HardPointAbs { get; }

        Vector3 TireWorldPosition { get; }
        Vector3 UpperConstraintPoint { get; }
        Vector3 LowerConstraintPoint { get; }
        Vector3 NotGroundedWheelPosition { get; }

        IVehicleAxle Axle { get; set; }
        float SteerAngle { get; set; }

        bool IsIdler { get; }

        abstract void IInitializable.Initialize();

    }
}
