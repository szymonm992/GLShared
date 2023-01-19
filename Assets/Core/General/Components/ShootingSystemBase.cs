using GLShared.General.Interfaces;
using GLShared.General.Signals;
using GLShared.Networking.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public abstract class ShootingSystemBase : MonoBehaviour, IInitializable, IShootingSystem
    {
        [Inject] protected readonly IPlayerInputProvider inputProvider;
        [Inject] protected readonly SignalBus signalBus;
        [Inject] protected readonly PlayerEntity playerEntity;
        [Inject] protected readonly ITurretController turretController;

        protected float currentReloadTimer = 0;
        protected bool isReloading = true;

        public bool ShootkingKeyPressed { get; set; }

        public virtual void Initialize()
        {
            
        }

        protected virtual void Update()
        {
            ShootkingKeyPressed = inputProvider.ShootingKey;

            if (currentReloadTimer > 0f)
            {
                currentReloadTimer -= Time.deltaTime;
            }
            else
            {
                currentReloadTimer = 0f;
                isReloading = false;
            }
        }

        protected void AfterShotCallback(float value)
        {
            currentReloadTimer = value;
        }
    }
}
