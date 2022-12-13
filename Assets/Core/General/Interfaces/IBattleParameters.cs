using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface IBattleParameters 
    {
        Func<int, bool> AreAllPlayersSpawned { get; }
        int DemandedPlayersSpawnedAmount { get; }
        float CountdownTime { get; }

    }
}
