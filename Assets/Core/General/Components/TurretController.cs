using Frontend.Scripts;
using GLShared.General.Interfaces;
using GLShared.General.Signals;
using GLShared.Networking.Components;
using GLShared.Networking.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public class TurretController : MonoBehaviour, IInitializable, ITurretController
    {
        [Inject] private readonly IVehicleController vehicleController;
        [Inject] private readonly IMouseActionsProvider mouseActionsProvider;
        [Inject] private readonly IPlayerInputProvider inputProvider;
        [Inject] private readonly ISyncManager syncManager;
        [Inject] private readonly SignalBus signalBus;
        [Inject] private readonly PlayerEntity playerEntity;

        [SerializeField] private Transform turret;
        [SerializeField] private Transform gun;

        private float turretRotationSpeed;
        private float gunRotationSpeed;
        private float gunDepression;
        private float gunElevation;

        private bool turretLock;
        private bool stabilizeTurret;
        private bool stabilizeGun;

        private Vector3 targetingWorldSpacePosition;
        private Vector3 targetVector;

        private Quaternion? previousGunRotation;
        private Quaternion? previousTurretRotation;

        public Transform Gun => gun;
        public Transform Turret => turret;
        public bool TurretLock => turretLock; 
        

        public void Initialize()
        {
            signalBus.Subscribe<PlayerSignals.OnPlayerInitialized>(OnLocalPlayerInitialized);
        }

        public void RotateTurret()
        { 
            Vector3 turretDiff = transform.InverseTransformVector(targetVector);

            if (turretDiff != Vector3.zero)
            {
                float desiredLocalAngleY = Quaternion.LookRotation(turretDiff).eulerAngles.y;

                float localAngleY = turret.localRotation.eulerAngles.y;
                if (stabilizeTurret && previousTurretRotation != null)
                {
                    var previousLocalRotationDiff = Quaternion.Inverse(previousTurretRotation.GetValueOrDefault()) * turret.rotation;

                    var angDiffOfPreviousAngleY = Mathf.DeltaAngle(0, previousLocalRotationDiff.eulerAngles.y);
                    var angDiffOfDesiredAngleY = Mathf.DeltaAngle(0, localAngleY - desiredLocalAngleY);

                    if (angDiffOfPreviousAngleY * angDiffOfDesiredAngleY > 0.0f)
                    {
                        localAngleY -= angDiffOfPreviousAngleY;
                    }
                }

                localAngleY = Mathf.MoveTowardsAngle(localAngleY, desiredLocalAngleY, Time.deltaTime * turretRotationSpeed);
                turret.localRotation = Quaternion.Euler(0.0f, localAngleY, 0.0f);
            }

        }

        public void RotateGun()
        {
            if (gun == null)
            {
                return;
            }

            Vector3 gunDesiredDirection = targetingWorldSpacePosition - gun.position;
            float gunDotProduct = Vector3.Dot(gun.forward, gunDesiredDirection.normalized);

            if (gunDotProduct != 1.0f)
            {
                Vector3 gunDiff = turret.InverseTransformVector(gunDesiredDirection);

                var desiredAngleX = Quaternion.LookRotation(gunDiff).eulerAngles.x;
                desiredAngleX = desiredAngleX.ClampAngle(-gunElevation, gunDepression);

                float localAngleX = gun.localRotation.eulerAngles.x;
                if (stabilizeGun && previousGunRotation != null)
                {
                    var previousLocalRotationDiff = Quaternion.Inverse(gun.rotation) * previousGunRotation.GetValueOrDefault();
                    localAngleX += previousLocalRotationDiff.eulerAngles.x;
                    localAngleX = localAngleX.ClampAngle(-gunElevation, gunDepression);
                }

                localAngleX = Mathf.MoveTowardsAngle(localAngleX, desiredAngleX, Time.deltaTime * gunRotationSpeed);
                gun.localRotation = Quaternion.Euler(localAngleX, 0.0f, 0.0f);
            }
        }

        private void OnLocalPlayerInitialized(PlayerSignals.OnPlayerInitialized OnLocalPlayerInitialized)
        {
            turretRotationSpeed = OnLocalPlayerInitialized.VehicleStats.TurretRotationSpeed;
            gunRotationSpeed = OnLocalPlayerInitialized.VehicleStats.GunRotationSpeed;

            gunDepression = OnLocalPlayerInitialized.VehicleStats.GunDepression;
            gunElevation = OnLocalPlayerInitialized.VehicleStats.GunElevation;

            stabilizeGun = OnLocalPlayerInitialized.VehicleStats.StabilizeGun;
            stabilizeTurret = OnLocalPlayerInitialized.VehicleStats.StabilizeTurret;
        }

        private void CacheControllerRotations()
        {
            previousTurretRotation = turret.rotation;
            previousGunRotation = gun.rotation;
        }

        private void LateUpdate()
        {
            if (vehicleController != null && !turretLock && !vehicleController.IsUpsideDown)
            {
                RotateTurret();
                RotateGun();
                playerEntity.CurrentTransform.Update(this);
            }

            CacheControllerRotations();
        }

        private void Update()
        {
            if (vehicleController == null)
            {
                return;
            }

            turretLock = inputProvider.TurretLockKey;

            if (!turretLock)
            {
                targetingWorldSpacePosition = mouseActionsProvider.CameraTargetingPosition;
                targetVector = targetingWorldSpacePosition - transform.position;
            } 
        }
    }
}
