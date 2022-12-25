using UnityEngine;

namespace GLShared.General.Models
{
    public class PlayerInput
    {
        public string Username { get; private set; }
        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }
        public bool Brake { get; private set; }
        public bool TurretLockKey { get; private set; }
        public Vector3 CameraTargetingPosition { get; private set; }

        public PlayerInput(string username, float horizontal, float vertical, bool brake, 
            bool turretLockKey, Vector3 cameraTargetingPosition)
        {
            Username = username;
            Horizontal = horizontal;
            Vertical = vertical;
            Brake = brake;
            TurretLockKey = turretLockKey;
            CameraTargetingPosition = cameraTargetingPosition;

        }

        public void UpdateCameraTarget(Vector3 cameraTargetingPosition)
        {
            CameraTargetingPosition = cameraTargetingPosition;
        }

        public void UpdateControllerInputs(float horizontal, float vertical, bool brake,
            bool turretLockKey)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            Brake = brake;
            TurretLockKey = turretLockKey;
        }
    }
}
