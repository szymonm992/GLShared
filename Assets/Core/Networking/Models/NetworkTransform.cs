using GLShared.General.Interfaces;
using GLShared.Networking.Interfaces;
using UnityEngine;

namespace GLShared.Networking.Models
{
    public class NetworkTransform : INetworkTransform
    {
        public string Identifier { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation => Quaternion.Euler(EulerAngles);
        public float GunAngleX { get; set; }
        public float TurretAngleY { get; set; }
        public float TargetGunAngleX { get; set; }
        public float TargetTurretAngleY { get; set; }
        public float CurrentSpeed { get; set; }
        public float CurrentTurningSpeed { get; set; }
        public long TimeStamp { get; set; }
        public Vector3 EulerAngles { get; set; }

        public void Update(Transform transform, float speed, float turnSpeed)
        {
            this.Position = transform.position;
            this.EulerAngles = transform.eulerAngles;
            this.CurrentSpeed = speed;
            this.CurrentTurningSpeed = turnSpeed;
        }

        public void Update(ITurretController turretController)
        {
            this.TurretAngleY = turretController.Turret.localEulerAngles.y;
            this.GunAngleX = turretController.Gun.localEulerAngles.x;
            this.TargetGunAngleX = turretController.TargetGunAngle;
            this.TargetTurretAngleY = turretController.TargetTurretAngle;
        }

        public bool HasChanged(NetworkTransform transform, float positionDifferenceThreshold, float rotationDifferenceThreshold)
        {
            return Vector3.Distance(Position, transform.Position) > positionDifferenceThreshold
                || Vector3.Distance(EulerAngles, transform.EulerAngles) > rotationDifferenceThreshold;
        }
    }
}

