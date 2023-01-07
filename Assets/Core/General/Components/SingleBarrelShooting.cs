using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Components
{
    public class SingleBarrelShooting : ShootingSystemBase
    {
        private float reloadTime = 5f;

        public override void Initialize()
        {
            base.Initialize();
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
                AfterShotCallback(reloadTime);
            }
        }

        protected void SingleShotLogic()
        {

        }
    }
}
