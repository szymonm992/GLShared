using Zenject;

namespace GLShared.General.Interfaces
{
    public interface IShellEntryInfo 
    {
        public string ShellId { get; }
        public string ShellName { get; }
        public GameObjectContext ShellPrefab { get; }
    }
}
