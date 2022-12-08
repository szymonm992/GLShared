using Automachine.Scripts.Components;
using GLShared.General.Enums;
using GLShared.General.Interfaces;
using GLShared.General.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public class BattleCountdownStage : State<BattleStage>
    {
        [Inject] private readonly IBattleParameters battleParameters;

        private float currentTimer = 0f;
        private bool finishedCountdown = false;

        public bool FinishedCountdown => finishedCountdown;

        public override void Tick()
        {
            base.Tick();

            if(isActive && !finishedCountdown)
            {
                if (currentTimer < battleParameters.CountdownTime)
                {
                    currentTimer += Time.deltaTime;
                }
                else
                {
                    currentTimer = battleParameters.CountdownTime;
                    finishedCountdown = true;
                }
            }       
        }
    }
}