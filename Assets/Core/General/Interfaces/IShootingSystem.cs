using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface IShootingSystem
    {
        Vector3 ShellSpawnEulerAngles { get; }
        Vector3 ShellSpawnPosition { get; }
        
        bool ShootkingKeyPressed { get; set; }

        (Vector3, float) GetGunTargetingPosition();
    }
}
