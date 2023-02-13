using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Models
{
    [System.Serializable]
    public class GroundFrictionPair
    {
        [SerializeField] private TerrainLayer[] layers;
        [SerializeField] private bool isDefaultLayer = false;

        [Range(0f, 1f)]
        [SerializeField] private float friction;
        [Range(0f, 1f)]
        [SerializeField] private float steeringMultiplier = 1f;
        [SerializeField] private RangedFloat anglesRange;

        public RangedFloat HorizontalAnglesRange => anglesRange;
        public IEnumerable<TerrainLayer> Layers => layers;
        public bool IsDefaultLayer => isDefaultLayer;
        public float SteeringMultiplier => steeringMultiplier;

        public float GetFrictionForAngle(float angle)
        {
            if (anglesRange.Min <= angle)
            {
                return friction;
            }
            else
            {
                return 0f;
            }
        }
    }
}
