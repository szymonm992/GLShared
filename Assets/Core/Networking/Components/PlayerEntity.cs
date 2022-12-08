using GLShared.General.Interfaces;
using GLShared.General.Models;
using GLShared.Networking.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GLShared.Networking.Components
{
    public class PlayerEntity : NetworkEntity, IInitializable
    {
        [Inject] private readonly GameObjectContext context;

        [SerializeField] private bool isLocalPlayer;

        private PlayerProperties playerProperties;

        public bool IsLocalPlayer => isLocalPlayer;
        public PlayerProperties PlayerProperties => playerProperties;

        [Inject]
        public void Construct(PlayerProperties propertiesAtPrefab)
        {
            propertiesAtPrefab.PlayerContext = context;
            UpdateProperties(propertiesAtPrefab);
        }

        public void UpdateProperties(PlayerProperties properties)
        {
            playerProperties = properties;

            transform.SetPositionAndRotation(playerProperties.SpawnPosition, playerProperties.SpawnRotation);
            isLocalPlayer = playerProperties.IsLocal;
        }

        public void Initialize()
        {

        }

    }
}
