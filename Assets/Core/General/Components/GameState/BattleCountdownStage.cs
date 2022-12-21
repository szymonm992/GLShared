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

        private float currentTimer = 0f;
        private bool finishedCountdown = false;
        private int previousIntTimer = 30;
        private int currentIntTimer = 30;

        public bool FinishedCountdown => finishedCountdown;
        public float CurrentTimer => currentTimer;
        public int PreviousIntegerTimer => previousIntTimer;
        public int CurrentIntegerTimer => currentIntTimer;

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
                previousIntTimer = (int)currentTimer;
                if (currentTimer > 0)
                {
                    currentTimer -= Time.deltaTime;
                }
                else
                {
                    currentTimer = 0;
                    finishedCountdown = true;
                }
                currentIntTimer = (int)currentTimer;
            }       
        }
    }
}
