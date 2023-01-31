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
        public string ShellId { get; set; }
        public string Username { get; set; }
        public bool IsInitialized { get; set; } = false;
    }
}
