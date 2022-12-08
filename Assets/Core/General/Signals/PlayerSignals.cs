using GLShared.General.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GLShared.General.Signals
{
    public class PlayerSignals
    {
        public class OnPlayerSpawned
        {
            public PlayerProperties PlayerProperties { get; set; }
        }

        public class OnPlayerInitialized
        {
            public PlayerProperties PlayerProperties { get; set; }
            public float TurretRotationSpeed { get; set; }
            public float GunRotationSpeed { get; set; }
            public float GunDepression { get; set; }
            public float GunElevation { get; set; }
        }
    }
}


