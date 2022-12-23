using GLShared.Networking.Components;
using GLShared.Networking.Interfaces;
using Sfs2X.Entities;
using Sfs2X.Protocol.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GLShared.General.Models
{
    public class PlayerProperties : SerializableSFSType
    {
        public GameObjectContext PlayerContext { get; set; }
        public Vector3 SpawnPosition { get; set; }
        public Quaternion SpawnRotation { get; set; }
        public string PlayerVehicleName { get; set; }
        public bool IsLocal { get; set; }
        public User User { get; set; }
    }
}
