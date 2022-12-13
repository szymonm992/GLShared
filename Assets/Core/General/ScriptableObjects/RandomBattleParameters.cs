using GLShared.General.Interfaces;
using System;
using UnityEngine;

namespace GLShared.General.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RandomBattleParameters", menuName = "UT/Battle parameters/Random battle parameters")]
    public class RandomBattleParameters : ScriptableObject, IBattleParameters
    {
        [Header("General parameters")]
        [SerializeField] private float countdownTime = 2f;
        [SerializeField] private int demandedPlayersSpawnedAmount = 1;
        public float CountdownTime => countdownTime;
        public int DemandedPlayersSpawnedAmount => demandedPlayersSpawnedAmount;

        public Func<int, bool> AreAllPlayersSpawned => ArePlayersSpawned;

        public bool ArePlayersSpawned(int currentPlayersAmount)
        {
            return currentPlayersAmount == demandedPlayersSpawnedAmount;
        }
    }
}
