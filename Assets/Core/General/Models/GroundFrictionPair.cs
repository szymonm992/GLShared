using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

        public float Friction => friction;
        public float SteeringMultiplier => steeringMultiplier;
        public RangedFloat HorizontalAnglesRange => anglesRange;
        

        public IEnumerable<TerrainLayer> Layers => layers;
        public bool IsDefaultLayer => isDefaultLayer;

        public float GetFrictionForAngle(float angle)
        {
            float returnFriction = 1f;

            if (anglesRange.Min <= angle)
            {
                returnFriction = friction;
            }
            else
            {
                returnFriction = 0f;
            }

            return returnFriction;
        }
    }
}
