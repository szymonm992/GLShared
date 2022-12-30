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
        public double Length { get; set; } // New field to store precalculated length
        public Vector3 EulerAngles { get; set; }


        public Vector3 PreviousPosition { get; set; }
        public Vector3 PreviousEulerAngles { get; set; }
        public float PreviousCurrentSpeed { get; set; }
        public float PreviousTurretAngleY { get; set; }
        public float PreviousGunAngleX { get; set; }

        public NetworkTransform()
        {
        }

        public NetworkTransform(NetworkTransform previousTransform, double timeStamp)
        {
            TimeStamp = timeStamp;
            Length = TimeStamp - previousTransform.TimeStamp;
        }

        public void Update(Transform transform, float speed)
        {
            // Update the current values
            this.Position = transform.position;
            this.EulerAngles = transform.eulerAngles;
            this.CurrentSpeed = speed;

            // Check if any of the relevant variables have changed
            if (Position != PreviousPosition || EulerAngles != PreviousEulerAngles ||
                CurrentSpeed != PreviousCurrentSpeed)
            {
                // Update the previous values
                PreviousPosition = Position;
                PreviousEulerAngles = EulerAngles;
                PreviousCurrentSpeed = CurrentSpeed;
            }
        }

        public void Update(ITurretController turretController)
        {
            // Update the current values
            this.TurretAngleY = turretController.Turret.localEulerAngles.y;
            this.GunAngleX = turretController.Gun.localEulerAngles.x;

            // Check if any of the relevant variables have changed
            if (TurretAngleY != PreviousTurretAngleY || GunAngleX != PreviousGunAngleX)
            {
                // Update the previous values
                PreviousTurretAngleY = TurretAngleY;
                PreviousGunAngleX = GunAngleX;
            }
        }

        public bool HasChanged(NetworkTransform transform, float positionDifferenceThreshold, float rotationDifferenceThreshold)
        {
            return Vector3.Distance(Position, transform.Position) > positionDifferenceThreshold
                || Vector3.Distance(EulerAngles, transform.EulerAngles) > rotationDifferenceThreshold
                || PreviousTurretAngleY != TurretAngleY
                || PreviousGunAngleX != GunAngleX;
        }
    }
}

