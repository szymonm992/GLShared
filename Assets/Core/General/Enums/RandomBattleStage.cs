using Automachine.Scripts.Attributes;
using GLShared.General.Components;

namespace GLShared.General.Enums
{
    [AutomachineStates]
    public enum BattleStage 
    {
        [DefaultState, StateEntity(typeof(BattleBeginningStage))]
        Beginning = 0,
        [StateEntity(typeof(BattleCountdownStage))]
        Countdown = 1,
        InProgress,
        Ending
    }
}
