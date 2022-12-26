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
        [Inject(Optional =  true)] private readonly ISyncInterpolator syncInterpolator;
        [Inject] private readonly IPlayerInputProvider inputProvider;

        [SerializeField] private bool isLocalPlayer;

        private PlayerProperties playerProperties;
        private PlayerInput playerInput;

        public bool IsLocalPlayer => isLocalPlayer;
        public PlayerProperties Properties => playerProperties;
        public PlayerInput Input => playerInput;
        public IPlayerInputProvider InputProvider => inputProvider;


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

            playerInput = new(playerProperties.User.Name, 0, 0, true, true, Vector3.zero);
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

            currentNetworkTransform = newNetworkTransform;
            base.ReceiveSyncPosition(currentNetworkTransform);
            syncInterpolator.ProcessCurrentNetworkTransform(currentNetworkTransform);
        }

        public override void Initialize()
        {
            base.Initialize();
            signalBus.Subscribe<PlayerSignals.OnPlayerInitialized>(OnPlayerInitialized);
        }

        protected override void Update()
        {
            base.Update();
            entityVelocity = isSender ? vehicleController.CurrentSpeed : Properties.IsInitialized ? currentNetworkTransform.CurrentSpeed : 0;
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
