using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Signals
{
    public class PlayerSignals
    {
        public class OnLocalPlayerInitialized
        {
            public float TurretRotationSpeed { get; set; }
            public float GunRotationSpeed { get; set; }
            public float GunDepression { get; set; }
            public float GunElevation { get; set; }
        }
    }
}


