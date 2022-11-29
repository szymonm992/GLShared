using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface ITurretController
    {
        bool TurretLock { get; }
        void RotateTurret();
    }
}
