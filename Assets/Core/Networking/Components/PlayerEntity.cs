using GLShared.General.Interfaces;
using GLShared.General.Models;
using GLShared.General.Signals;
using GLShared.Networking.Interfaces;
using GLShared.Networking.Models;
using UnityEngine;
using Zenject;

namespace GLShared.Networking.Components
{
    public class PlayerEntity : NetworkEntity
    {
        private const string LOCAL_PLAYER_NAME = "localPlayer";

        [Inject] private readonly IPlayerInstaller playerInstaller;
        [Inject] private readonly GameObjectContext context;
        [Inject] private readonly IVehicleController vehicleController;
        [Inject] private readonly IPlayerInputProvider inputProvider;
        [Inject(Optional = true)] private readonly ISyncInterpolator syncInterpolator;
        [Inject(Optional = true)] private readonly ITurretController turretController;

        private bool isLocalPlayer;
        private PlayerProperties playerProperties;
        private PlayerInput playerInput;

        public bool IsLocalPlayer => isLocalPlayer;
        public PlayerProperties Properties => playerProperties;
        public PlayerInput Input => playerInput;
        public IPlayerInputProvider InputProvider => inputProvider;
        public string Username => currentNetworkTransform.Username;

        [Inject]
        public void Construct(PlayerProperties propertiesAtPrefab)
        {
            propertiesAtPrefab.PlayerContext = context;
            UpdateProperties(propertiesAtPrefab);

            currentNetworkTransform = new()
            {
                Position = transform.position,
                EulerAngles = transform.eulerAngles,
                GunAngleX = vehicleController.HasTurret ? turretController.Gun.localEulerAngles.x : 0f,
                TurretAngleY = vehicleController.HasTurret ? turretController.Turret.localEulerAngles.y : 0f,
                TimeStamp = 0d,
                CurrentSpeed = EntityVelocity,
                Username = playerInstaller.IsPrototypeInstaller ? LOCAL_PLAYER_NAME : Properties.User.Name,
            };

            playerInput = new (playerInstaller.IsPrototypeInstaller ? LOCAL_PLAYER_NAME : playerProperties.User.Name, 0f, 0f, 0f, true, true, Vector3.zero, false);
        }

        public void UpdateProperties(PlayerProperties properties)
        {
            playerProperties = properties;

            transform.SetPositionAndRotation(playerProperties.SpawnPosition, playerProperties.SpawnRotation);
            isLocalPlayer = playerProperties.IsLocal;
        }

        public override void SendSyncPosition()
        {
            base.SendSyncPosition();
            currentNetworkTransform.Update(transform, EntityVelocity);
            syncManager.SyncPosition(this);
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
            entityVelocity = GetLocalControllerSpeed();
        }

        private void OnPlayerInitialized(PlayerSignals.OnPlayerInitialized OnPlayerInitialized)
        {
            if (!playerInstaller.IsPrototypeInstaller)
            {
                if (OnPlayerInitialized.PlayerProperties.User.Name == playerProperties.User.Name)
                {
                    playerProperties.IsInitialized = true;
                }
            }
            else
            {
                playerProperties.IsInitialized = true;
            }
        }

        private float GetLocalControllerSpeed()
        {
            if (playerInstaller.IsPrototypeInstaller)
            {
                return vehicleController.CurrentSpeed;
            }
            else
            {
                return isSender ? vehicleController.CurrentSpeed : Properties.IsInitialized ? currentNetworkTransform.CurrentSpeed : 0f;
            }
        }
    }
}
