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
        [Inject] protected readonly ShellSpawner shellSpawner;
        [Inject] protected readonly SmartFoxConnection smartFox;

        protected readonly Dictionary<string, PlayerEntity> connectedPlayers = new();
        protected readonly Dictionary<string, ShellEntity> shells = new();

        protected int spawnedPlayersAmount;
        protected int spawnedShellsAmount;
        protected double currentServerTime;

        public int SpawnedPlayersAmount => spawnedPlayersAmount;
        public double CurrentServerTime => currentServerTime;

        public virtual void Initialize()
        {
        }

        public virtual void SyncPosition(PlayerEntity _)
        {
        }

        public virtual void SyncInputs(PlayerInput _)
        {
        }

        public virtual void SyncShell(ShellEntity _)
        {

        }

        public void TryCreateShell(string username, string shellId, string identifier, Vector3 spawnPosition, Vector3 spawnEulerAngles)
        {
            if (connectedPlayers.ContainsKey(username) && connectedPlayers[username].ShootingSystem != null)
            {
                CreateShell(username, shellId, identifier, spawnPosition, spawnEulerAngles);
            }
        }

        public void TryCreatePlayer(string username, Vector3 spawnPosition, Vector3 spawnEulerAngles)
        {
            if (!connectedPlayers.ContainsKey(username))
            {
                CreatePlayer(username, spawnPosition, spawnEulerAngles, out _);
            }
        }

        protected virtual void CreatePlayer(string username, Vector3 spawnPosition, Vector3 spawnEulerAngles, out PlayerProperties playerProperties)
        {
            var user = smartFox.Connection.UserManager.GetUserByName(username);

            if (user == null)
            {
                Debug.LogError($"User '{username}' has not been found in User Manager");
                playerProperties = null;
                return;
            }

            if (!user.ContainsVariable(NetworkConsts.VAR_PLAYER_VEHICLE))
            {
                Debug.LogError($"User does not contain '{NetworkConsts.VAR_PLAYER_VEHICLE}' variable");
                playerProperties = null;
                return;
            }

            string vehicleName = user.GetVariable(NetworkConsts.VAR_PLAYER_VEHICLE).Value.ToString();
            playerProperties = GetPlayerInitData(username, vehicleName, spawnPosition, spawnEulerAngles);

            if (playerProperties == null)
            {
                Debug.LogError("Could not create an init player data from given parameters");
                return;
            }

            var prefabEntity = playerProperties.PlayerContext.gameObject.GetComponent<PlayerEntity>();//this references only to prefab
            var playerEntity = playerSpawner.Spawn(prefabEntity, playerProperties);

            connectedPlayers.Add(username, playerEntity);
            spawnedPlayersAmount++;
        }

        protected virtual void CreateShell(string username, string shellId, string identifier, Vector3 spawnPosition, Vector3 spawnEulerAngles)
        {
            var shellProperties = GetShellInitData(username, shellId, identifier, spawnPosition, spawnEulerAngles);

            if (shellProperties == null)
            {
                Debug.LogError("Could not create an init shell data from given parameters");
                return;
            }

            var prefabEntity = shellProperties.ShellContext.gameObject.GetComponent<ShellEntity>();//this references only to prefab
            var shellEntity = shellSpawner.Spawn(prefabEntity, shellProperties);

            //TODO: consider whether having this in dictionary makes any sense
            //TODO: Generate an unique id for shell (cuz shellid is repetitive) we need to generate some index
            shells.Add(shellProperties.ShellId, shellEntity);
            spawnedShellsAmount++;

            Debug.Log($"Player {username} has shot a shell with id {spawnedShellsAmount}");
        }

        protected virtual PlayerProperties GetPlayerInitData(string username, string vehicleName,
            Vector3 spawnPosition, Vector3 spawnEulerAngles)
        {
            return null;
        }

        protected ShellProperties GetShellInitData(string username, string shellId, string identifier,
            Vector3 spawnPosition, Vector3 spawnEulerAngles)
        {
            var shellData = shellsDatabase.GetShellInfo(shellId);

            if (shellData != null)
            {
                return new()
                {
                    ShellContext = shellData.ShellPrefab,
                    ShellId = shellId,
                    SpawnPosition = spawnPosition,
                    SpawnRotation = Quaternion.Euler(spawnEulerAngles.x, spawnEulerAngles.y, spawnEulerAngles.z),
                    Username = username,
                    Identifier = identifier,
                };
            }

            return null;
        }

        protected virtual void Update()
        {
            
        }
    }
}
