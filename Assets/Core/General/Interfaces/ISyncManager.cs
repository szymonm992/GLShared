using GLShared.Networking.Interfaces;
using Sfs2X.Entities;
using UnityEngine;
using Zenject;

namespace GLShared.General.Interfaces
{
    public interface ISyncManager : IInitializable
    {
        double CurrentServerTime { get; }
        int SpawnedPlayersAmount { get; }

        void TryCreatePlayer(User user, Vector3 spawnPosition, Vector3 spawnEulerAngles);

        void SyncPosition(INetworkEntity networkEntity);
    }
}
