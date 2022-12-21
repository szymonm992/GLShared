using GLShared.General.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface IBattleManager
    {
        public BattleStage CurrentBattleStage { get; }
    }
}
