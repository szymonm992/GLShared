using UnityEngine;

namespace GLShared.General.Models
{
    [System.Serializable]
    public class RangedFloat
    {
        public RangedFloat(float min = float.NegativeInfinity, float max = float.PositiveInfinity)
        {
            this.min = min;
            this.max = max;
        }
        public float min;
        public float max;

        public float difference => max - min;

        public float length => max - min;
        public float GetPercent(float value)
        {
            return (value - min) / length;
        }

        public bool InRange(float value)
        {
            return value >= min && value <= max;
        }
        public float GetRandom()
        {
            return Random.Range(min, max);
        }
      
        public static bool InRange(float value, float min, float max)
        {
            return value >= min && value <= max;
        }
        public float Clamp(float v)
        {
            return Mathf.Clamp(v, min, max);
        }
    }
} 