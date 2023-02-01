using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GLShared.General.Interfaces
{
    public interface IShellStats
    {
        float Speed { get; }
        float Penetration { get; }
        float Damage { get; }
        float Caliber { get; }
        float GravityMultiplier { get; }
    }
}
