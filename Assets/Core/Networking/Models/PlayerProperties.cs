using Sfs2X.Entities;
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
        public string Username { get; set; }
        public string Team { get; set; }
        public bool IsInitialized { get; set; } = false;
    }
}
