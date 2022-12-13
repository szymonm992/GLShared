using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface IBattleParameters 
    {
        int DemandedPlayersSpawnedAmount { get; }
        float CountdownTime { get; }
    }
}
