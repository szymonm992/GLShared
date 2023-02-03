using GLShared.General.Interfaces;
using GLShared.General.Models;

namespace GLShared.General.Signals
{
    public class ShellSignals
    {
        public class OnShellSpawned
        {
            public ShellProperties ShellProperties { get; set; }
        }

        public class OnShellInitialized
        {
            public int SceneId { get; set; }
        }
    }
}


