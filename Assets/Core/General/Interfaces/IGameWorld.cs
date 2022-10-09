using System;

namespace GLShared.General.Interfaces
{
    public interface IGameWorld
    {
        event Action OnWorldStateInitialized;
    }
}
