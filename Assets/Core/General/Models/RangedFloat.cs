using UnityEngine;

namespace GLShared.General.Models
{
    [System.Serializable]
    public class RangedFloat
    {
        [SerializeField] private float min;
        [SerializeField] private float max;
        
        public float Max
        {
            get => max;
            set => max = value;
        }

        public float Min
        {
            get => min;
            set => min = value;
        }

        public float Difference => max - min;
        public float Length => max - min;

        public RangedFloat(float min = float.NegativeInfinity, float max = float.PositiveInfinity)
        {
            this.min = min;
            this.max = max;
        }

        public static bool InRange(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        public float GetPercent(float value)
        {
            return (value - min) / Length;
        }

        public float GetMidPoint()
        {
            return (min + max) * 0.5f;
        }

        public bool InRange(float value)
        {
            return value >= min && value <= max;
        }

        public float GetRandom()
        {
            return Random.Range(min, max);
        }
        
        public float Clamp(float value)
        {
            return Mathf.Clamp(value, min, max);
        }
    }
} 