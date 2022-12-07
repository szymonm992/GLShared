using Automachine.Scripts.Components;
using GLShared.General.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Components
{
    public class BattleInProgressStage : State<BattleStage>
    {
        public override void StartState()
        {
            base.StartState();
            Debug.Log("The battle just started!");
        }
    }
}
