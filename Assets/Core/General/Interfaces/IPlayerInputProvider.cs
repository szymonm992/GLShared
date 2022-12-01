using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Interfaces
{ 
    public interface IPlayerInputProvider 
    {
        public float Vertical { get; }
        public float Horizontal { get; }
        public bool Brake { get; }

        public float CombinedInput { get; }
        public float SignedVertical { get; }
        public float SignedHorizontal { get; }
        public float AbsoluteVertical { get; }
        public float AbsoluteHorizontal { get; }
        public float LastVerticalInput { get; }
        public bool SnipingKey { get; }
        public bool TurretLockKey { get; }
    }
}