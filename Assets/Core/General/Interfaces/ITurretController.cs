using GLShared.Networking.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface ITurretController
    {
        Transform Turret { get; }
        Transform Gun { get; }
        bool TurretLock { get; }
        float TargetTurretAngle { get; }
        float TargetGunAngle { get; }

        void RotateTurret();
        void RotateGun();
    }
}
