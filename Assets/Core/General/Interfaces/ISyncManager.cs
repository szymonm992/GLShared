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

        void TryCreatePlayer(string username, Vector3 spawnPosition, Vector3 spawnEulerAngles);

        void TryCreateShell(string username, string databaseId, int sceneIdentifier,
            Vector3 spawnPosition, Vector3 spawnEulerAngles, (Vector3, float) targetingProperties);

        void TryDestroyingShell(int sceneIdentifier);

        void SyncPosition(PlayerEntity playerEntity);

        void SyncInputs(PlayerInput playerInput);

        void SyncShell(ShellEntity shellEntity);
    }
}
