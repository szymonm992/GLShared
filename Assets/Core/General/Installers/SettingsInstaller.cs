using GLShared.General.Interfaces;
using GLShared.General.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace GLShared.General.Installers
{
    public class SettingsInstaller : MonoInstaller
    {
        [SerializeField] private VehiclesDatabase vehiclesDatabase;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<IVehiclesDatabase>().FromInstance(vehiclesDatabase).AsCached();
        }
    }
}
