using GLShared.Networking.Interfaces;
using UnityEngine;

namespace GLShared.Networking.Models
{
    public class NetworkShellTransform : INetworkTransform
    {
        public string Identifier { get; set; }
        public Vector3 Position { get; set; }
        public float CurrentSpeed { get; set; }
        public double TimeStamp { get; set; }
        public Vector3 EulerAngles { get; set; }
        public Quaternion Rotation => Quaternion.Euler(EulerAngles);

        public void Update(Transform transform, float speed)
        {
            this.Position = transform.position;
            this.EulerAngles = transform.eulerAngles;
            this.CurrentSpeed = speed;
        }
    }
}

