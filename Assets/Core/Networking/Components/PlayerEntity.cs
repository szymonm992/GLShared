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
        [Inject(Optional = true)] private readonly IShootingSystem shootingSystem;

        private bool isLocalPlayer;
        private PlayerProperties playerProperties;
        private PlayerInput playerInput;
        private NetworkTransform currentNetworkTransform;

        public bool IsLocalPlayer => isLocalPlayer;
        public PlayerProperties Properties => playerProperties;
        public PlayerInput Input => playerInput;
        public IPlayerInputProvider InputProvider => inputProvider;
        public IShootingSystem ShootingSystem => shootingSystem;
        
        public string Username => currentNetworkTransform.Identifier;
        public NetworkTransform CurrentTransform => currentNetworkTransform;

        [Inject]
        public void Construct(PlayerProperties propertiesAtPrefab)
        {
            propertiesAtPrefab.PlayerContext = context;
            UpdateProperties(propertiesAtPrefab);

            currentNetworkTransform = new NetworkTransform()
            {
                Position = transform.position,
                EulerAngles = transform.eulerAngles,
                GunAngleX = vehicleController.HasTurret ? turretController.Gun.localEulerAngles.x : 0f,
                TurretAngleY = vehicleController.HasTurret ? turretController.Turret.localEulerAngles.y : 0f,
                TimeStamp = 0d,
                CurrentSpeed = EntityVelocity,
                Identifier = playerInstaller.IsPrototypeInstaller ? LOCAL_PLAYER_NAME : Properties.Username,
            };

            playerInput = new (playerInstaller.IsPrototypeInstaller ? LOCAL_PLAYER_NAME : playerProperties.Username, 0f, 0f, 0f, true, true, Vector3.zero, false);
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

        public override void ReceiveSyncPosition(INetworkTransform newNetworkTransform)
        {
            if (isSender)
            {
                return;
            }

            currentNetworkTransform = (NetworkTransform)newNetworkTransform;
            base.ReceiveSyncPosition(currentNetworkTransform);
            syncInterpolator.ProcessCurrentNetworkTransform(currentNetworkTransform);
        }

        public override void Initialize()
        {
            base.Initialize();

            if (currentNetworkTransform == null)
            {
                currentNetworkTransform = new NetworkTransform()
                {
                    Position = transform.position,
                    EulerAngles = transform.eulerAngles,
                    GunAngleX = 0,
                    TurretAngleY = 0,
                    TimeStamp = 0d,
                    CurrentSpeed = EntityVelocity,
                    Identifier = NETWORK_ENTITY_DEFAULT_VALUE,
                };
            }
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
                if (OnPlayerInitialized.PlayerProperties.Username == playerProperties.Username)
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
