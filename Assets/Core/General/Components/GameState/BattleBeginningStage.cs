using Automachine.Scripts.Components;
using GLShared.General.Enums;
using GLShared.General.Interfaces;
using GLShared.General.Signals;
using GLShared.Networking.Components;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public class BattleBeginningStage : State<BattleStage>
    {
        [Inject] private readonly ISyncManager syncManager;
        [Inject] private readonly SmartFoxConnection smartFox;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void StartState()
        {
            base.StartState();
        }
    }
}
