using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface IVehicleAxle
    {
        public IEnumerable<IPhysicsWheel> AllWheels { get; }
        public IEnumerable<IPhysicsWheel> GroundedWheels { get; }

        public bool CanDrive { get; }
        public bool CanSteer { get; }
        public bool InvertSteer { get; }
        public bool HasAnyWheelPair { get; }
        public bool HasAnyWheel { get; }
        public bool IsAxleGrounded { get; }
        abstract void SetSteerAngle(float angleLeft, float angleRight);
    }
}
