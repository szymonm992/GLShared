using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.Networking.Models
{
    public class NetworkTransform
    {
        public Vector3 Position { get; set; }
        public Vector3 EulerAngles { get; set; }
        public float CurrentSpeed { get; set; }
        public double TimeStamp { get; set; }

        public bool IsDifferent(Transform transform, float differenceThreshold)
        {
            bool isPositionDifferent = Vector3.Distance(Position, transform.position) > differenceThreshold;
            return isPositionDifferent || (Vector3.Distance(EulerAngles, transform.localEulerAngles) > differenceThreshold);
        }
    }
}
