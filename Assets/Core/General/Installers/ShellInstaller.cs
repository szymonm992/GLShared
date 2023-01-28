using GLShared.General.Interfaces;
using GLShared.Networking.Components;
using Zenject;

namespace GLShared.General.Installers
{
    public class ShellInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInitializableExecutionOrder<ShellEntity>(+10);
            Container.BindInitializableExecutionOrder<IShellController>(+20);
        }
    }
}
