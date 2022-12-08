using GLShared.Networking.Components;
using GLShared.Networking.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GLShared.General.Models
{
    public class PlayerProperties
    {
        public GameObjectContext PlayerContext { get; set; }
        public Vector3 SpawnPosition { get; set; }
        public Quaternion SpawnRotation { get; set; }
        public string PlayerVehicleName { get; set; }
        public bool IsLocal { get; set; }
    }
}
