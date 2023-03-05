using System;
using UnityEngine;

namespace GLShared.General.Models
{
    public class PlayerInput : IEquatable<PlayerInput>
    {
        public string Username { get; private set; }
        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }
        public float RawVertical { get; private set; }
        public bool Brake { get; private set; }
        public bool TurretLockKey { get; private set; }
        public bool ShootingKey { get; private set; }
        public Vector3 CameraTargetingPosition { get; private set; }

        public PlayerInput(string username, float horizontal, float vertical, float rawVertical, bool brake, 
            bool turretLockKey, Vector3 cameraTargetingPosition, bool shootingKey)
        {
            Username = username;
            Horizontal = horizontal;
            Vertical = vertical;
            RawVertical = rawVertical;
            Brake = brake;
            TurretLockKey = turretLockKey;
            CameraTargetingPosition = cameraTargetingPosition;
            ShootingKey = shootingKey;
        }

        public PlayerInput(float horizontal, float vertical, float rawVertical, bool brake, bool turretLockKey, bool shootingKey)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            RawVertical = rawVertical;
            Brake = brake;
            TurretLockKey = turretLockKey;
            ShootingKey = shootingKey;
        }

        //Constructor for remote clients
        public PlayerInput(string username, float horizontal, float vertical, float rawVertical)
        {
            Username = username;
            Horizontal = horizontal;
            Vertical = vertical;
            RawVertical = rawVertical;
        }

        public void UpdateCameraTarget(Vector3 cameraTargetingPosition)
        {
            CameraTargetingPosition = cameraTargetingPosition;
        }

        public void UpdateControllerInputs(float horizontal, float vertical, float rawVertical, bool brake,
            bool turretLockKey, bool shootingKey)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            RawVertical = rawVertical;
            Brake = brake;
            TurretLockKey = turretLockKey;
            ShootingKey = shootingKey;
        }

        public bool Equals(PlayerInput other)
        {
            if (other is null)
            {
                return this is null;
            }

            return this.Username == other.Username
                && Mathf.Approximately(this.Horizontal, other.Horizontal)
                && Mathf.Approximately(this.Vertical, other.Vertical)
                && Mathf.Approximately(this.RawVertical, other.RawVertical)
                && this.Brake == other.Brake
                && this.TurretLockKey == other.TurretLockKey
                && this.ShootingKey == other.ShootingKey
                && Mathf.Approximately(this.CameraTargetingPosition.x, other.CameraTargetingPosition.x)
                && Mathf.Approximately(this.CameraTargetingPosition.y, other.CameraTargetingPosition.y)
                && Mathf.Approximately(this.CameraTargetingPosition.z, other.CameraTargetingPosition.z);
        }

        public override bool Equals(object obj)
        {
            var other = obj as PlayerInput;
            if (other is null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Username, Horizontal, Vertical, RawVertical, Brake, TurretLockKey, ShootingKey, CameraTargetingPosition);
        }

        public static bool operator ==(PlayerInput input1, PlayerInput input2)
        {
            if (input1 is null)
            {
                return input2 is null;
            }

            return input1.Equals(input2);
        }

        public static bool operator !=(PlayerInput input1, PlayerInput input2)
        {
            if (input1 is null)
            {
                return input2 is not null;
            }

            return !input1.Equals(input2);
        }
    }
}
