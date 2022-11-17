using GLShared.General.Enums;
using GLShared.General.Models;
using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface IPhysicsWheel
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
        float SteerAngle { get; set; }
    }
}
