using GLShared.General.Interfaces;
using GLShared.General.Models;
using GLShared.Networking.Components;
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
        [SerializeField] private bool isLocalPlayer;

        private PlayerProperties playerProperties;
        public bool IsLocalPlayer => isLocalPlayer;
        public PlayerProperties PlayerProperties => playerProperties;
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
                Username = PlayerProperties.User.Name,
            };
        }

        public void UpdateProperties(PlayerProperties properties)
        {
            playerProperties = properties;

            transform.SetPositionAndRotation(playerProperties.SpawnPosition, playerProperties.SpawnRotation);
            isLocalPlayer = playerProperties.IsLocal;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

    }
}
