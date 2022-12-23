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
        public Vector3 SpawnPosition { get; set; }
        public Quaternion SpawnRotation { get; set; }
        public User User { get; set; }
    }
}
