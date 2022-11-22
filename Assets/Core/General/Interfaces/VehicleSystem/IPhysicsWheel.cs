using GLShared.General.Models;
using UnityEngine;
using Zenject;

namespace GLShared.General.Interfaces
{
    public interface IPhysicsWheel : IInitializable
    {
        Transform Transform { get; }
        bool IsGrounded { get; }
        HitInfo HitInfo { get; }
        float WheelRadius { get; }
        float TireMass { get; }
        float ForwardTireGripFactor { get; }
        float SidewaysTireGripFactor { get; }
        float CompressionRate { get; }
        float HardPointAbs { get; }

        Vector3 TireWorldPosition { get; }
        Vector3 UpperConstraintPoint { get; }
        Vector3 LowerConstraintPoint { get; }
        Vector3 NotGroundedWheelPosition { get; }

        float SteerAngle { get; set; }

        abstract void IInitializable.Initialize();

    }
}
