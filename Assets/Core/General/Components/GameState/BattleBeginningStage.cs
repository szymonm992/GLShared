using Automachine.Scripts.Components;
using GLShared.General.Enums;
using GLShared.General.Interfaces;
using GLShared.General.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public class BattleBeginningStage : State<BattleStage>
    {

        [Inject] private readonly ISyncManager syncManager;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void StartState()
        {
            base.StartState();
            syncManager.CreatePlayer(true, "T-55", new Vector3(132.35f, 2f, 118.99f), Quaternion.Euler(0, 90f, 0));
        }
    }
}
