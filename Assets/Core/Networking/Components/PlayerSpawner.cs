using GLShared.General.Models;
using GLShared.General.Signals;
using GLShared.Networking.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Frontend.Scripts
{
    public class PlayerSpawner : MonoBehaviour
    {
        [Inject] private readonly Factory playerFactory;
        [Inject] private readonly SignalBus signalBus;
        public class Factory : PlaceholderFactory<PlayerEntity, PlayerProperties, PlayerEntity>
        {

        }

        public class PlayerInstaller : Installer
        {
            private readonly PlayerProperties data;
            private readonly PlayerEntity prefab;

            public PlayerInstaller(PlayerEntity prefab, PlayerProperties data)
            {
                this.data = data;
                this.prefab = prefab;
            }

            public override void InstallBindings()
            {
                Container.Bind<PlayerProperties>().FromInstance(data).AsCached();
                Container.Bind<PlayerEntity>().FromComponentInNewPrefab(prefab).AsCached();
            }
        }

        public void Spawn(PlayerEntity prefab, PlayerProperties properties)
        {
            playerFactory.Create(prefab, properties);

            signalBus.Fire(new PlayerSignals.OnPlayerSpawned()
            {
                PlayerProperties = properties,
            });
        }
    }
}
