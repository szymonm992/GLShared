using UnityEngine;

namespace GLShared.General.Models
{
    [System.Serializable]
    public class AngleFrictionThreshold
    {
        [Range(0f, 1f)]
        [SerializeField] private float friction;
        [SerializeField] private RangedFloat anglesRange;

        public float Friction => friction;
        public RangedFloat AnglesRange => anglesRange;
    }
}
