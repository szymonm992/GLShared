using System;

namespace GLShared.General.Interfaces
{
    public interface IBattleParameters 
    {
        Func<int, bool> AreAllPlayersSpawned { get; }
        int DemandedPlayersSpawnedAmount { get; }
        float BattleStartCountdown { get; }
        float BattleDurationTime { get; }

    }
}
