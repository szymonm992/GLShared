using GLShared.General.Components;
using GLShared.General.Enums;
using GLShared.General.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Models
{
    [System.Serializable]
    public class UTAxlePairBase
    {
        [SerializeField] protected UTPhysicWheelBase wheel;
        [SerializeField] protected DriveAxisSite axis;

        protected bool isIdler = false;
        public DriveAxisSite Axis => axis;
        public IPhysicsWheel Wheel => wheel;
        public bool IsIdler => isIdler;

        public virtual void Initialize()
        {
            isIdler = wheel is UTIdlerWheel;
        }
    }
}
