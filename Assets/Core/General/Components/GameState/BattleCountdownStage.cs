using Automachine.Scripts.Components;
using GLShared.General.Enums;
using GLShared.General.Interfaces;
using GLShared.General.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public class BattleCountdownStage : State<BattleStage>
    {
        [Inject] private readonly IBattleParameters battleParameters;
        [Inject(Id = "countdownText")] private readonly TextMeshProUGUI countdownText;

        private float currentTimer = 0f;
        private bool finishedCountdown = false;

        public bool FinishedCountdown => finishedCountdown;

        public override void StartState()
        {
            base.StartState();
            currentTimer = battleParameters.CountdownTime;
        }

        public override void Tick()
        {
            base.Tick();

            if(isActive && !finishedCountdown)
            {
                if (currentTimer > 0)
                {
                    currentTimer -= Time.deltaTime;
                    countdownText.text = "Battle starts in: " + currentTimer.ToString("F0");
                }
                else
                {
                    currentTimer = 0;
                    countdownText.text = "Let's go";
                    finishedCountdown = true;
                }
            }       
        }
    }
}
