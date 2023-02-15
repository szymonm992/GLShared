using GLShared.General.Enums;
using GLShared.General.Models;
using GLShared.Networking.Components;
using UnityEngine;
using Zenject;

namespace GLShared.General.Interfaces
{
    public interface ISyncManager : IInitializable
    {
        double CurrentServerTime { get; }
        int SpawnedPlayersAmount { get; }

        void TryCreatePlayer(string username, Team team, Vector3 spawnPosition, Vector3 spawnEulerAngles);

        void TryCreateShell(string username, string databaseId, int sceneIdentifier,
            Vector3 spawnPosition, Vector3 spawnEulerAngles, (Vector3, float) targetingProperties);

        void TryDestroyingShell(int sceneIdentifier);

        void SyncPosition(PlayerEntity playerEntity);

        void SyncInputs(PlayerInput playerInput);

        void SyncShell(ShellEntity shellEntity);
    }
}
