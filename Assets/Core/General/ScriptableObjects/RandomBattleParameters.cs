using GLShared.General.Interfaces;
using UnityEngine;

namespace GLShared.General.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RandomBattleParameters", menuName = "UT/Battle parameters/Random battle parameters")]
    public class RandomBattleParameters : ScriptableObject, IBattleParameters
    {
        [Header("General parameters")]
        [SerializeField] private float countdownTime = 2f;
       
        public float CountdownTime => countdownTime;
    }
}
