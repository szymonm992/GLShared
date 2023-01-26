using GLShared.General.Interfaces;
using System;
using UnityEngine;

namespace GLShared.General.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RandomBattleParameters", menuName = "UT/Battle parameters/Random battle parameters")]
    public class RandomBattleParameters : ScriptableObject, IBattleParameters
    {
        [Header("General parameters")]
        [SerializeField] private float battleStartCountdown = 4f;
        [Tooltip("In seconds")]
        [SerializeField] private float battleDurationTime = 900f;
        [SerializeField] private int demandedPlayersSpawnedAmount = 1;

        public float BattleDurationTime => battleDurationTime;
        public float BattleStartCountdown => battleStartCountdown;
        public int DemandedPlayersSpawnedAmount => demandedPlayersSpawnedAmount;

        public Func<int, bool> AreAllPlayersSpawned => ArePlayersSpawned;

        public bool ArePlayersSpawned(int currentPlayersAmount)
        {
            return currentPlayersAmount == demandedPlayersSpawnedAmount;
        }
    }
}
