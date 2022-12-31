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
            Matrix4x4 parentMatrix = transform.worldToLocalMatrix;
            Vector3 turretDiff = parentMatrix * targetVector;

            if (turretDiff != Vector3.zero)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(turretDiff, transform.up);
                desiredRotation.eulerAngles = new Vector3(0.0f, desiredRotation.eulerAngles.y, 0.0f);

                Quaternion localRotation = turret.localRotation;
                if (stabilizeTurret && previousTurretRotation != null)
                {
                    var previousLocalRotation = Quaternion.Inverse(turret.parent.rotation) * previousTurretRotation.GetValueOrDefault();
                    var angDiffOfPrevious = Quaternion.Inverse(previousLocalRotation) * localRotation;
                    var angDiffOfDesired = Quaternion.Inverse(desiredRotation) * localRotation;

                    var angDiffOfPreviousAngleY = Mathf.DeltaAngle(0, angDiffOfPrevious.eulerAngles.y);
                    var angDiffOfDesiredAngleY = Mathf.DeltaAngle(0, angDiffOfDesired.eulerAngles.y);

                    if (angDiffOfPreviousAngleY * angDiffOfDesiredAngleY > 0.0f)
                    {
                        localRotation = previousLocalRotation;
                    }
                }

                turret.localRotation = Quaternion.RotateTowards(localRotation, desiredRotation, Time.deltaTime * turretRotationSpeed);

                playerEntity.CurrentNetworkTransform.Update(this);
            }

            previousTurretRotation = turret.rotation;
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
                var turrentMatrix = turret.worldToLocalMatrix;
                Vector3 gunDiff = turrentMatrix * gunDesiredDirection;
                var targetRotation = Quaternion.LookRotation(gunDiff, turret.up);
                targetRotation.eulerAngles = new Vector3(targetRotation.eulerAngles.x.ClampAngle(-gunElevation, gunDepression), 0, 0);

                Quaternion localRotation = gun.localRotation;
                if (stabilizeGun && previousGunRotation != null)
                {
                    localRotation = Quaternion.Inverse(turret.parent.rotation) * previousGunRotation.GetValueOrDefault();
                    localRotation.eulerAngles = new Vector3(localRotation.eulerAngles.x.ClampAngle(-gunElevation, gunDepression), 0, 0);
                }

                gun.localRotation = Quaternion.RotateTowards(localRotation, targetRotation, Time.deltaTime * gunRotationSpeed);

                playerEntity.CurrentNetworkTransform.Update(this);
            }

            previousGunRotation = gun.rotation;
        }

        private void OnLocalPlayerInitialized(PlayerSignals.OnPlayerInitialized OnLocalPlayerInitialized)
        {
            turretRotationSpeed = OnLocalPlayerInitialized.TurretRotationSpeed;
            gunRotationSpeed = OnLocalPlayerInitialized.GunRotationSpeed;

            gunDepression = OnLocalPlayerInitialized.GunDepression;
            gunElevation = OnLocalPlayerInitialized.GunElevation;

            stabilizeGun = OnLocalPlayerInitialized.StabilizeGun;
            stabilizeTurret = OnLocalPlayerInitialized.StabilizeTurret;
        }

        private void LateUpdate()
        {
            if (vehicleController == null || turretLock || vehicleController.IsUpsideDown)
            {
                return; 
            }

            RotateTurret();
            RotateGun();
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
