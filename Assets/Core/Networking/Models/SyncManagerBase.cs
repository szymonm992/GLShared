using GLShared.General.Components;
using GLShared.General.Interfaces;
using GLShared.General.Models;
using GLShared.Networking.Components;
using GLShared.Networking.Interfaces;
using Sfs2X.Entities;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GLShared.Networking.Models
{
    public abstract class SyncManagerBase : MonoBehaviour, ISyncManager
    {
        [Inject] protected readonly SignalBus signalBus;
        [Inject] protected readonly IVehiclesDatabase vehicleDatabase;
        [Inject] protected readonly IShellsDatabase shellsDatabase;
        [Inject] protected readonly PlayerSpawner playerSpawner;
        [Inject] protected readonly SmartFoxConnection smartFox;

        protected readonly Dictionary<string, PlayerEntity> connectedPlayers = new();
        protected readonly Dictionary<string, NetworkEntity> shells = new();

        protected int spawnedPlayersAmount;
        protected double currentServerTime;

        public int SpawnedPlayersAmount => spawnedPlayersAmount;
        public double CurrentServerTime => currentServerTime;

        public virtual void Initialize()
        {
        }

        public virtual void SyncPosition(INetworkEntity _)
        {
        }

        public virtual void SyncInputs(PlayerInput _)
        {
        }

        public virtual void SyncShell(IShellController _)
        {

        }

        public void TryCreateShell(string username, string shellId)
        {
            if (connectedPlayers.ContainsKey(username) && connectedPlayers[username].ShootingSystem != null)
            {
                CreateShell(username, shellId, connectedPlayers[username].ShootingSystem.ShellSpawnPosition, connectedPlayers[username].ShootingSystem.ShellSpawnEulerAngles);
            }
        }

        public void TryCreatePlayer(User user, Vector3 spawnPosition, Vector3 spawnEulerAngles)
        {
            if (!connectedPlayers.ContainsKey(user.Name))
            {
                CreatePlayer(user, spawnPosition, spawnEulerAngles, out _);
            }
        }

        protected virtual void CreatePlayer(User user, Vector3 spawnPosition, Vector3 spawnEulerAngles, out PlayerProperties playerProperties)
        {
            if (!user.ContainsVariable(NetworkConsts.VAR_PLAYER_VEHICLE))
            {
                Debug.LogError("User does not contain 'playerVehicle' variable");
                playerProperties = null;
                return;
            }
            
            var vehicleName = user.GetVariable(NetworkConsts.VAR_PLAYER_VEHICLE).Value.ToString();
            playerProperties = GetPlayerInitData(user, vehicleName, spawnPosition, spawnEulerAngles);

            if (playerProperties == null)
            {
                Debug.LogError("Could not create an init player data from given parameters");
                return;
            }

            var prefabEntity = playerProperties.PlayerContext.gameObject.GetComponent<PlayerEntity>();//this references only to prefab
            var playerEntity = playerSpawner.Spawn(prefabEntity, playerProperties);

            connectedPlayers.Add(user.Name, playerEntity);
            spawnedPlayersAmount++;
        }

        protected virtual void CreateShell(string username, string shellId, Vector3 spawnPosition, Vector3 spawnEulerAngles)
        {
            var shellPrefab = shellsDatabase.GetShellInfo(shellId);
            Debug.Log($"Player {username} has shot a shell");
        }

        protected virtual PlayerProperties GetPlayerInitData(User user, string vehicleName,
            Vector3 spawnPosition, Vector3 spawnEulerAngles)
        {
            return null;
        }

        protected virtual void Update()
        {
            
        }
    }
}
