using Sfs2X.Entities;
using UnityEngine;
using Zenject;

namespace GLShared.General.Models
{
    public class ShellProperties
    {
        public GameObjectContext ShellContext { get; set; }
        public Vector3 SpawnPosition { get; set; }
        public Quaternion SpawnRotation { get; set; }
        public string ShellId { get; set; } //id of shell in database
        public string Identifier { get; set; } //identifier used for sync
        public string Username { get; set; }
        public bool IsInitialized { get; set; } = false;
    }
}
