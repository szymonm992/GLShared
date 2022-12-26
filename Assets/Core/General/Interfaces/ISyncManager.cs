using GLShared.General.Models;
using GLShared.Networking.Components;
using GLShared.Networking.Interfaces;
using GLShared.Networking.Models;
using Sfs2X.Entities;
using System.Collections.Generic;
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

        void SyncInputs(PlayerInput playerInput);

        void SyncTurretTransform(ITurretController turretController)
    }
}
