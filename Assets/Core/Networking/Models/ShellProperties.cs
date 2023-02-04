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
        public (Vector3 point, float distance) TargetingProperties { get; set; }
        public string DatabaseId { get; set; } //id of shell in database
        public int ShellSceneIdentifier { get; set; } //identifier used for sync
        public string Username { get; set; }
        public bool IsInitialized { get; set; } = false;
    }
}
