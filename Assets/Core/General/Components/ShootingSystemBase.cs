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
        protected const string DEFAULT_SHELL_ID = "0";

        [Inject] protected readonly SignalBus signalBus;
        [Inject] protected readonly PlayerEntity playerEntity;
        [Inject] protected readonly ITurretController turretController;
        [Inject] protected readonly IPlayerInputProvider inputProvider;

        protected float currentReloadTimer = 0;
        protected bool isReloading = true;
        protected Vector3 shellSpawnPosition;
        protected Vector3 shellSpawnEulerAngles;

        public bool ShootkingKeyPressed { get; set; }
        public Vector3 ShellSpawnEulerAngles => shellSpawnEulerAngles;
        public Vector3 ShellSpawnPosition => shellSpawnPosition;

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
