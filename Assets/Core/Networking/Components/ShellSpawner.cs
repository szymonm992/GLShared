using GLShared.General.Models;
using GLShared.General.Signals;
using GLShared.Networking.Components;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public class ShellSpawner : MonoBehaviour
    {
        [Inject] private readonly Factory shellFactory;
        [Inject] private readonly SignalBus signalBus;

        public class Factory : PlaceholderFactory<ShellEntity, ShellProperties, ShellEntity>
        {
        }

        public class ShellInstaller : Installer
        {
            private readonly ShellProperties data;
            private readonly ShellEntity prefab;

            public ShellInstaller(ShellEntity prefab, ShellProperties data)
            {
                this.data = data;
                this.prefab = prefab;
            }

            public override void InstallBindings()
            {
                Container.Bind<ShellProperties>().FromInstance(data).AsCached();
                Container.Bind<ShellEntity>().FromComponentInNewPrefab(prefab).AsCached();
            }
        }

        public ShellEntity Spawn(ShellEntity prefab, ShellProperties properties)
        {
            var newShell = shellFactory.Create(prefab, properties);
            newShell.transform.SetLocalPositionAndRotation(properties.SpawnPosition, properties.SpawnRotation);

            return newShell;
        }
    }
}
