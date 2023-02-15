using GLShared.General.Signals;
using UnityEngine;

namespace GLShared.General.Components
{
    public class SingleBarrelShooting : ShootingSystemBase
    {
        [SerializeField] private float reloadTime = 5f;
        [SerializeField] private Transform shellSpawnPivot;

        public override void Initialize()
        {
            base.Initialize();

            AfterShotCallback(reloadTime);
        }

        protected override void Update()
        {
            if (inputProvider.LockPlayerInput)
            {
                return;
            }

            base.Update();

            if (ShootkingKeyPressed && !isReloading)
            {
                SingleShotLogic();
            }
        }

        protected void SingleShotLogic()
        {
            isReloading = true;

            shellSpawnEulerAngles = turretController.Gun.eulerAngles;

            signalBus.Fire(new PlayerSignals.OnPlayerShot()
            {
                Username = playerEntity.Properties.Username,
                ShellId = DEFAULT_SHELL_ID,
                ShellSpawnPosition = shellSpawnPivot.position,
                ShellSpawnEulerAngles = shellSpawnEulerAngles,
                TargetingProperties = GetGunTargetingPosition(),
            });

            AfterShotCallback(reloadTime);
        }
    }
}
