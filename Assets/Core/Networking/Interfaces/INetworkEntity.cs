using GLShared.Networking.Enums;
using Zenject;

namespace GLShared.Networking.Interfaces
{
    public interface INetworkEntity : IInitializable
    {
        NetworkEntityType EntityType { get; }
        bool IsSender { get; }
        float EntityVelocity { get; }
        INetworkTransform CurrentNetworkTransform { get; }

        void SendSyncPosition();
    }
}
