using GLShared.Networking.Enums;
using GLShared.Networking.Models;

namespace GLShared.Networking.Interfaces
{
    public interface INetworkEntity
    {
        NetworkEntityType EntityType { get; }
        bool IsPlayer { get; }
        bool IsSender { get; }
        float EntityVelocity { get; }
        INetworkTransform CurrentNetworkTransform { get; }

        void SendSyncPosition();
    }
}
