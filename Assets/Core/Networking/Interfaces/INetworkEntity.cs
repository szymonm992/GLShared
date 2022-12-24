using GLShared.Networking.Enums;
using GLShared.Networking.Models;

namespace GLShared.Networking.Interfaces
{
    public interface INetworkEntity
    {
        public NetworkEntityType EntityType { get; }
        public bool IsPlayer { get; }
        public float EntityVelocity { get; }
        public NetworkTransform CurrentNetworkTransform { get; }
        void SyncPosition();
    }
}
