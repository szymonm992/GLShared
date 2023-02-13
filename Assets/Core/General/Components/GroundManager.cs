using GLShared.General.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public class GroundManager : MonoBehaviour, IInitializable
    {
        public const float DEFAULT_FRICTION_VALUE = 1f;

        [SerializeField] private List<GroundFrictionPair> groundFrictionList;

        public void Initialize()
        {
            
        }

        public float GetFrictionForLayer(string layerName, float angle)
        {
            if (groundFrictionList.Any())
            {
                foreach (var pair in groundFrictionList)
                {
                    var matchingLayer = pair.Layers.FirstOrDefault(layer => layer.name == layerName);

                    if (matchingLayer != null)
                    {
                        return pair.GetFrictionForAngle(angle);
                    }
                }
            }

            return DEFAULT_FRICTION_VALUE;
        }

        public GroundFrictionPair GetPair(string layerName)
        {
            if (groundFrictionList.Any())
            {
                foreach (var pair in groundFrictionList)
                {
                    var matchingLayer = pair.Layers.FirstOrDefault(layer => layer.name == layerName);

                    if (matchingLayer != null)
                    {
                        return pair;
                    }
                }
            }

            return null;
        }
    }
}
