using GLShared.General.Enums;
using GLShared.General.Interfaces;
using GLShared.General.Models;
using UnityEngine;

namespace GLShared.General
{
    public static class UTWheelExtensions
    {
        public static Vector3 ReturnWheelPoint(this IPhysicsWheel wheel, ForceApplyPoint forceApplyPoint)
        {
            if (forceApplyPoint == ForceApplyPoint.WheelConstraintUpperPoint)
            {
                return wheel.UpperConstraintPoint;
            }
            else if (forceApplyPoint == ForceApplyPoint.WheelConstraintLowerPoint)
            {
                return wheel.LowerConstraintPoint;
            }
            else
            {
                return wheel.HitInfo.Point;
            }
        }
    }
}
