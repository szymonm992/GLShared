using Automachine.Scripts.Components;
using GLShared.General.Enums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public class BattleInProgressStage : State<BattleStage>
    {
        [Inject(Id = "countdownText")] private readonly TextMeshProUGUI countdownText;

        public override void StartState()
        {
            base.StartState();
            Debug.Log("The battle just started!");
            StartCoroutine(HideBeginningText(3f));
        }

        private IEnumerator HideBeginningText(float delay)
        {
            yield return new WaitForSeconds(delay);
            countdownText.text = "";
        }
    }
}
