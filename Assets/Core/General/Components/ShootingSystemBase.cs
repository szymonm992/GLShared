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

        protected float currentReloadTimer = 0;
        protected bool isReloading = true;

        public bool ShootkingKeyPressed { get; set; }

        public virtual void Initialize()
        {
            
        }

        protected virtual void Update()
        {
            ShootkingKeyPressed = inputProvider.ShootingKey;

            if (currentReloadTimer > 0)
            {
                currentReloadTimer -= Time.deltaTime;
            }
            else
            {
                currentReloadTimer = 0;
                isReloading = false;
            }
        }

        protected void AfterShotCallback(float value)
        {
            signalBus.Fire(new PlayerSignals.OnPlayerShot()
            {
                Username = playerEntity.Properties.User.Name,
                ShellId = "0",
            });

            currentReloadTimer = value;
            isReloading = true;
        }
    }
}
