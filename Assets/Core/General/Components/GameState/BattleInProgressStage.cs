using Automachine.Scripts.Components;
using GLShared.General.Enums;
using GLShared.General.Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public class BattleInProgressStage : State<BattleStage>
    {
        [Inject] private readonly IBattleParameters battleParameters;

        private bool haveSecondsChanged = false;
        private float currentTimer = 0f;
        private int currentSeconds = 0;
        private int previousSeconds = 0;
        public bool HaveSecondsChanged => haveSecondsChanged;
        public int SecondsLeft => currentSeconds;
        public int MinutesLeft => (int)currentTimer / 60;

        public override void StartState()
        {
            base.StartState();

            currentTimer = battleParameters.BattleDurationTime;
            currentSeconds = (int)currentTimer % 60;
            haveSecondsChanged = true;
        }

        public override void Tick()
        {
            base.Tick();

            if (currentTimer > 0f)
            {
                currentTimer -= Time.deltaTime;
                currentSeconds = (int)currentTimer % 60;
            }
            else
            {
                currentSeconds = 0;
                currentTimer = 0;
            }

            haveSecondsChanged = previousSeconds != currentSeconds;
            if (haveSecondsChanged)
            {
                previousSeconds = currentSeconds;
            }
        }
    }
}
