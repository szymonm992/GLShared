using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.Networking.Interfaces
{
    public interface INetworkTransform
    {
        string Identifier { get; }
        Vector3 Position { get; }
        float CurrentSpeed { get; }
        double TimeStamp { get; }
        Vector3 EulerAngles { get; }
        Quaternion Rotation { get; }

        abstract void Update(Transform transform, float speed);
    }
}
