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
        [SerializeField] private AngleFrictionThreshold[] angleFrictionThresholds;
        [SerializeField] private bool isDefaultLayer = false;

        public IEnumerable<TerrainLayer> Layers => layers;
        public bool IsDefaultLayer => isDefaultLayer;

        public float GetFrictionForAngle(float angle)
        {
            float friction = 1f;

            foreach (var pair in angleFrictionThresholds)
            {
                if (pair.AnglesRange.Min <= angle)
                {
                    friction = pair.Friction;
                }
                else
                {
                    friction = 0f;
                }
            }

            return friction;
        }
    }
}
