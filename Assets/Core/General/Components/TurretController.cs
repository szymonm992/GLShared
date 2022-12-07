using Frontend.Scripts;
using GLShared.General.Interfaces;
using GLShared.General.Signals;
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
        [Inject] private readonly SignalBus signalBus;

        [SerializeField] private Transform turret;
        [SerializeField] private Transform gun;

        private float turretRotationSpeed;
        private float gunRotationSpeed;
        private float gunDepression;
        private float gunElevation;

        private bool turretLock;

        private Vector3 targetingWorldSpacePosition;
        private Vector3 targetVector;

        public float lastTurretEulerY;

        public Transform Gun => gun;

        public bool TurretLock
        {
            get => turretLock; 
        }

        public void Initialize()
        {
            lastTurretEulerY = turret.eulerAngles.y;
            signalBus.Subscribe<PlayerSignals.OnLocalPlayerInitialized>(OnLocalPlayerInitialized);
        }

        public void RotateTurret()
        {
            if (turret.eulerAngles.y != lastTurretEulerY)
            {
                turret.eulerAngles = new Vector3(turret.eulerAngles.x, lastTurretEulerY, turret.eulerAngles.z);
            }
                
            Matrix4x4 parentMatrix = transform.worldToLocalMatrix;
            Vector3 turretDiff = parentMatrix * targetVector;

            Quaternion desiredRotation = Quaternion.LookRotation(turretDiff, transform.up);
            desiredRotation.eulerAngles = new Vector3(0, desiredRotation.eulerAngles.y, 0);

            turret.localRotation = Quaternion.RotateTowards(turret.localRotation, desiredRotation, Time.deltaTime * turretRotationSpeed);

            lastTurretEulerY = turret.eulerAngles.y;
        }

        public void RotateGun()
        {
            if (gun != null)
            {
                Vector3 gunDesiredPosition = targetingWorldSpacePosition - gun.position;
                Matrix4x4 turrentMatrix = turret.worldToLocalMatrix;
                Vector3 gun_diff = turrentMatrix * gunDesiredPosition;

                var rotation = Quaternion.LookRotation(gun_diff, turret.up);
                
                rotation.eulerAngles = new Vector3(rotation.eulerAngles.x.ClampAngle(-gunElevation, gunDepression), 0, 0);
                gun.localRotation = Quaternion.RotateTowards(gun.localRotation, rotation, Time.deltaTime * gunRotationSpeed);
            }
        }

        private void OnLocalPlayerInitialized(PlayerSignals.OnLocalPlayerInitialized OnLocalPlayerInitialized)
        {
            turretRotationSpeed = OnLocalPlayerInitialized.TurretRotationSpeed;
            gunRotationSpeed = OnLocalPlayerInitialized.GunRotationSpeed;

            gunDepression = OnLocalPlayerInitialized.GunDepression;
            gunElevation = OnLocalPlayerInitialized.GunElevation;
        }

        private void LateUpdate()
        {
            if (turretLock || vehicleController.IsUpsideDown)
            {
                return; 
            }

            RotateTurret();
            RotateGun();
        }

        private void Update()
        {
            turretLock = inputProvider.TurretLockKey;

            if (!turretLock)
            {
                targetingWorldSpacePosition = mouseActionsProvider.CameraTargetingPosition;
                targetVector = targetingWorldSpacePosition - transform.position;
            } 
        }
    }
}
