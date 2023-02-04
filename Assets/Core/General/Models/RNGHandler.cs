using GLShared.General.Interfaces;
using UnityEngine;

namespace GLShared.General.Models
{
    public class RNGHandler : IRNGHandler
    {
        public float RandomGaussian(float minimumValue = 0f, float maximumValue = 1f)
        {
            float u;
            float v;
            float S;

            do
            {
                u = 2f * Random.value - 1f;
                v = 2f * Random.value - 1f;
                S = (u * u) + (v * v); 
            }
            while (S >= 1.0f);

            float standardNormalDistribution = u * Mathf.Sqrt(-2f * Mathf.Log(S) / S);

            float mean = (minimumValue + maximumValue) * 0.5f;
            float sigma = (maximumValue - mean) / 3f;

            return Mathf.Clamp(standardNormalDistribution * sigma + mean, minimumValue, maximumValue);
        }
    }
}
