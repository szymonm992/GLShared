using GLShared.General.Components;
using GLShared.General.Interfaces;
using GLShared.General.Models;
using GLShared.General.Signals;
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
        protected readonly Dictionary<int, ShellEntity> shells = new();

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

        public virtual void TryDestroyingShell(int sceneIdentifier)
        {
            if (shells.ContainsKey(sceneIdentifier))
            {
                signalBus.Fire(new ShellSignals.OnShellDestroyed()
                {
                    ShellSceneId = sceneIdentifier,
                });

                shells.Remove(sceneIdentifier);
            }
        }

        public void TryCreateShell(string username, string databaseId, int sceneIdentifier, Vector3 spawnPosition, Vector3 spawnEulerAngles)
        {
            if (connectedPlayers.ContainsKey(username) && connectedPlayers[username].ShootingSystem != null)
            {
                CreateShell(username, databaseId, sceneIdentifier, spawnPosition, spawnEulerAngles, out _);
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

        protected virtual void CreateShell(string username, string databaseId, int sceneId, Vector3 spawnPosition, Vector3 spawnEulerAngles, out ShellProperties shellProperties)
        {
            shellProperties = GetShellInitData(username, databaseId, sceneId, spawnPosition, spawnEulerAngles);

            if (shellProperties == null)
            {
                Debug.LogError("Could not create an init shell data from given parameters");
                return;
            }

            var prefabEntity = shellProperties.ShellContext.gameObject.GetComponent<ShellEntity>();//this references only to prefab
            var shellEntity = shellSpawner.Spawn(prefabEntity, shellProperties);

            spawnedShellsAmount++;
            shells.Add(sceneId, shellEntity);

            Debug.Log($"Player {username} has shot a shell of id ({shellProperties.DatabaseId}) with network id ({shellProperties.ShellSceneIdentifier})");
        }

        protected virtual PlayerProperties GetPlayerInitData(string username, string vehicleName,
            Vector3 spawnPosition, Vector3 spawnEulerAngles)
        {
            return null;
        }

        protected ShellProperties GetShellInitData(string username, string databaseId, int sceneIdentifier,
            Vector3 spawnPosition, Vector3 spawnEulerAngles)
        {
            var shellData = shellsDatabase.GetShellInfo(databaseId);

            if (shellData != null)
            {
                return new()
                {
                    ShellContext = shellData.ShellPrefab,
                    DatabaseId = databaseId,
                    SpawnPosition = spawnPosition,
                    SpawnRotation = Quaternion.Euler(spawnEulerAngles.x, spawnEulerAngles.y, spawnEulerAngles.z),
                    Username = username,
                    ShellSceneIdentifier = sceneIdentifier,
                };
            }

            return null;
        }

        protected virtual void Update()
        {
            
        }
    }
}
