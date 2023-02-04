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
        protected const float MAX_SHELL_FLIGHT_DISTANCE = 1000f;

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

        public (Vector3, float) GetGunTargetingPosition()
        {
            if (Physics.Raycast(new Ray(turretController.Gun.position, turretController.Gun.forward), out RaycastHit hit,  MAX_SHELL_FLIGHT_DISTANCE))
            {
                return new (hit.point, hit.distance);
            }
            return new (turretController.Gun.position + (turretController.Gun.forward * 1000f), 1000f);
        }

        protected virtual void Update()
        {
            ShootkingKeyPressed = inputProvider.ShootingKey;

            if (currentReloadTimer > 0.0f)
            {
                currentReloadTimer -= Time.deltaTime;
            }
            else
            {
                currentReloadTimer = 0.0f;
                isReloading = false;
            }
        }

        protected void AfterShotCallback(float value)
        {
            currentReloadTimer = value;
        }
    }
}
