using GLShared.General.Interfaces;
using GLShared.Networking.Components;
using Zenject;

namespace GLShared.General.Installers
{
    public class ShellInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInitializableExecutionOrder<IShellController>(+10);
            Container.BindInitializableExecutionOrder<ShellEntity>(+20);
            
        }
    }
}
