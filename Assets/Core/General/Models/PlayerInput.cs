using UnityEngine;

namespace GLShared.General.Models
{
    public class PlayerInput
    {
        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }
        public bool Brake { get; private set; }
        public bool TurretLockKey { get; private set; }
        public Vector3 CameraTargetingPosition { get; private set; }

        public PlayerInput(float horizontal, float vertical, bool brake, 
            bool turretLockKey, Vector3 cameraTargetingPosition)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            Brake = brake;
            TurretLockKey = turretLockKey;
            CameraTargetingPosition = cameraTargetingPosition;

        }
    }
}
