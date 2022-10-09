using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using GLShared.General.Interfaces;

namespace GLShared.General.Models
{
    public class GameWorld : IGameWorld, IInitializable
    {
        public event Action OnWorldStateInitialized;

        public void Initialize()
        {
            OnWorldStateInitialized?.Invoke();
        }

    }
}
