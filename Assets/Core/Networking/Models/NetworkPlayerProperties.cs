using Sfs2X.Entities;
using Sfs2X.Protocol.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GLShared.Networking.Models
{
    public class NetworkPlayerProperties : SerializableSFSType
    {
        public float SpawnPositionX { get; }
        public float SpawnPositionY { get; }
        public float SpawnPositionZ { get; }

        public float SpawnEulerX { get; }
        public float SpawnEulerY { get; }
        public float SpawnEulerZ { get; }
        public User User { get; }

        public NetworkPlayerProperties(Vector3 spawnPosition, Quaternion spawnRotation, User user)
        {
            this.SpawnPositionX = spawnPosition.x;
            this.SpawnPositionY = spawnPosition.y;
            this.SpawnPositionZ = spawnPosition.z;

            this.SpawnEulerX = spawnRotation.eulerAngles.x;
            this.SpawnEulerY = spawnRotation.eulerAngles.y;
            this.SpawnEulerZ = spawnRotation.eulerAngles.z;

            this.User = user;
        }
    }
}
