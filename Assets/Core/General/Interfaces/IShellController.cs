using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GLShared.General.Interfaces
{
    public interface IShellController : IInitializable
    {
        public string OwnerUsername { get; }
        public float Velocity { get; }
    }
}
