using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.Networking.Models
{
    public class NetworkTransform
    {
        public string Username { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 EulerAngles { get; set; }
        public float CurrentSpeed { get; set; }
        public double TimeStamp { get; set; }
        
        public void Update(Transform transform, float speed)
        {
            this.Position = transform.position;
            this.EulerAngles = transform.eulerAngles;
            this.CurrentSpeed = speed;
        }

        public bool HasChanged(Transform transform, float differenceThreshold)
        {
            bool isPositionDifferent = Vector3.Distance(Position, transform.position) > differenceThreshold;
            return isPositionDifferent || (Vector3.Distance(EulerAngles, transform.localEulerAngles) > differenceThreshold);
        }
    }
}
