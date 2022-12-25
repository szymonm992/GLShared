using GLShared.General.Interfaces;
using GLShared.General.Models;
using GLShared.General.Signals;
using GLShared.Networking.Components;
using GLShared.Networking.Interfaces;
using GLShared.Networking.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace GLShared.Networking.Components
{
    public class PlayerEntity : NetworkEntity
    {
        [Inject] private readonly GameObjectContext context;
        [Inject] private readonly IVehicleController vehicleController;
        [Inject] private readonly ISyncInterpolator syncInterpolator;

        [SerializeField] private bool isLocalPlayer;

        private PlayerProperties playerProperties;

        public bool IsLocalPlayer => isLocalPlayer;
        public PlayerProperties Properties => playerProperties;
        public override float EntityVelocity => vehicleController.CurrentSpeed;

        [Inject]
        public void Construct(PlayerProperties propertiesAtPrefab)
        {
            propertiesAtPrefab.PlayerContext = context;
            UpdateProperties(propertiesAtPrefab);

            currentNetworkTransform = new()
            {
                Position = transform.position,
                EulerAngles = transform.eulerAngles,
                TimeStamp = 0d,
                CurrentSpeed = EntityVelocity,
                Username = Properties.User.Name,
            };
        }

        public void UpdateProperties(PlayerProperties properties)
        {
            playerProperties = properties;

            transform.SetPositionAndRotation(playerProperties.SpawnPosition, playerProperties.SpawnRotation);
            isLocalPlayer = playerProperties.IsLocal;
        }

        public override void ReceiveSyncPosition(NetworkTransform newNetworkTransform)
        {
            if (isSender)
            {
                return;
            }

            base.ReceiveSyncPosition(newNetworkTransform);
            if (currentNetworkTransform.HasChanged(newNetworkTransform, 0.001f))
            {
                currentNetworkTransform = newNetworkTransform;
                syncInterpolator.ProcessCurrentNetworkTransform(currentNetworkTransform);
            }
        }
        public override void Initialize()
        {
            base.Initialize();
            signalBus.Subscribe<PlayerSignals.OnPlayerInitialized>(OnPlayerInitialized);
        }

        private void OnPlayerInitialized(PlayerSignals.OnPlayerInitialized OnPlayerInitialized)
        {
            if(OnPlayerInitialized.PlayerProperties.User.Name == playerProperties.User.Name)
            {
                playerProperties.IsInitialized = true;
            }
        }    
    }
}
