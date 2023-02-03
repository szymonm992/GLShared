using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GLShared.General.Interfaces
{
    public interface IShellController : IInitializable, IDisposable
    {
        public float Velocity { get; }
    }
}
