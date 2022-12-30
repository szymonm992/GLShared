using GLShared.General.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.Networking.Models
{
    public class NetworkTransform
    {
        public string Username { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation => Quaternion.Euler(EulerAngles);
        public float GunAngleX { get; set; }
        public float TurretAngleY { get; set; }
        public float CurrentSpeed { get; set; }
        public double TimeStamp { get; set; }
        public Vector3 EulerAngles { get; set; }



        public void Update(Transform transform, float speed)
        {
            this.Position = transform.position;
            this.EulerAngles = transform.eulerAngles;
            this.CurrentSpeed = speed;
        }

        public void Update(ITurretController turretController)
        {
            // Update the current values
            this.TurretAngleY = turretController.Turret.localEulerAngles.y;
            this.GunAngleX = turretController.Gun.localEulerAngles.x;
        }

        public bool HasChanged(NetworkTransform transform, float positionDifferenceThreshold, float rotationDifferenceThreshold)
        {
            return Vector3.Distance(Position, transform.Position) > positionDifferenceThreshold
                || Vector3.Distance(EulerAngles, transform.EulerAngles) > rotationDifferenceThreshold;
        }
    }
}

