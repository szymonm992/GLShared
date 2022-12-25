using Frontend.Scripts;
using GLShared.General.Interfaces;
using GLShared.General.Models;
using GLShared.Networking.Components;
using GLShared.Networking.Interfaces;
using Sfs2X.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using Zenject;

namespace GLShared.Networking.Models
{
    public abstract class SyncManagerBase : MonoBehaviour, ISyncManager
    {
        [Inject] protected readonly SignalBus signalBus;
        [Inject] protected readonly IVehiclesDatabase vehicleDatabase;
        [Inject] protected readonly PlayerSpawner playerSpawner;
        [Inject] protected readonly SmartFoxConnection smartFox;

        protected readonly Dictionary<string, PlayerEntity> connectedPlayers = new Dictionary<string, PlayerEntity>();

        protected int spawnedPlayersAmount = 0;
        protected double currentServerTime = 0;

        public int SpawnedPlayersAmount => spawnedPlayersAmount;
        public double CurrentServerTime => currentServerTime;

        public virtual void Initialize()
        {
        }

        public virtual void SyncPosition(INetworkEntity _)
        {
        }

        public void TryCreatePlayer(User user, Vector3 spawnPosition, Vector3 spawnEulerAngles)
        {
            if (!connectedPlayers.ContainsKey(user.Name))
            {
                CreatePlayer(user, spawnPosition, spawnEulerAngles);
            }
        }

        protected virtual void CreatePlayer(User user, Vector3 spawnPosition, Vector3 spawnEulerAngles)
        {
            var vehicleName = user.GetVariable("playerVehicle").Value.ToString();
            var playerProperties = GetPlayerInitData(user, vehicleName, spawnPosition, spawnEulerAngles);
            var prefabEntity = playerProperties.PlayerContext.gameObject.GetComponent<PlayerEntity>();//this references only to prefab
            var playerEntity = playerSpawner.Spawn(prefabEntity, playerProperties);

            connectedPlayers.Add(user.Name, playerEntity);
            spawnedPlayersAmount++;
        }

        protected virtual PlayerProperties GetPlayerInitData(User user, string vehicleName,
            Vector3 spawnPosition, Vector3 spawnEulerAngles)
        {
           
            return null;
        }
    }
}