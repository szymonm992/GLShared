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

        private Vector3 targetingWorldSpacePosition;
        private Vector3 targetVector;

        private NetworkTurretTransform currentNetworkTransform;

        public Transform Gun => gun;
        public NetworkTurretTransform CurrentNetworkTransform => currentNetworkTransform;
        public bool TurretLock
        {
            get => turretLock; 
        }

        public void Initialize()
        {
            signalBus.Subscribe<PlayerSignals.OnPlayerInitialized>(OnLocalPlayerInitialized);

            currentNetworkTransform = new NetworkTurretTransform()
            {
                Username = playerEntity.Properties.User.Name,
                GunAnglesX = gun.localEulerAngles.x,
                TurretAnglesY = turret.localEulerAngles.y,
            };
        }

        public void RotateTurret()
        { 
            Matrix4x4 parentMatrix = transform.worldToLocalMatrix;
            Vector3 turretDiff = parentMatrix * targetVector;

            if (turretDiff != Vector3.zero)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(turretDiff, transform.up);
                desiredRotation.eulerAngles = new Vector3(0, desiredRotation.eulerAngles.y, 0);
                turret.localRotation = Quaternion.RotateTowards(turret.localRotation, desiredRotation, Time.deltaTime * turretRotationSpeed);
                currentNetworkTransform.TurretAnglesY = turret.localEulerAngles.y;
            }
        }

        public void RotateGun()
        {
            if (gun != null)
            {
                Vector3 gunDesiredDirection = targetingWorldSpacePosition - gun.position;
                float gunDotProduct = Vector3.Dot(gun.forward, gunDesiredDirection.normalized);

                if (gunDotProduct == 1)
                {
                    var turrentMatrix = turret.worldToLocalMatrix;
                    Vector3 gunDiff = turrentMatrix * gunDesiredDirection;
                    var rotation = Quaternion.LookRotation(gunDiff, turret.up);

                    rotation.eulerAngles = new Vector3(rotation.eulerAngles.x.ClampAngle(-gunElevation, gunDepression), 0, 0);
                    gun.localRotation = Quaternion.RotateTowards(gun.localRotation, rotation, Time.deltaTime * gunRotationSpeed);
                    currentNetworkTransform.GunAnglesX = gun.localEulerAngles.x;
                }
            }
        }

        private void OnLocalPlayerInitialized(PlayerSignals.OnPlayerInitialized OnLocalPlayerInitialized)
        {
            turretRotationSpeed = OnLocalPlayerInitialized.TurretRotationSpeed;
            gunRotationSpeed = OnLocalPlayerInitialized.GunRotationSpeed;

            gunDepression = OnLocalPlayerInitialized.GunDepression;
            gunElevation = OnLocalPlayerInitialized.GunElevation;
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

            if(currentNetworkTransform.NeedsUpdate)
            {
                syncManager.SyncTurretTransform(this);
                currentNetworkTransform.NeedsUpdate = false;
            }
        }
    }
}
