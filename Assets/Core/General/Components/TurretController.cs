using GLShared.General.Interfaces;
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

        [SerializeField] private float turretRotationSpeed = 48f;
        [SerializeField] private Transform turret;
        
        private bool turretLock;
        private Vector3 targetingWorldSpacePosition;
        private Vector3 targetVector;
        private Quaternion lastRotation;

        public bool TurretLock
        {
            get => turretLock; 
        }

        public void Initialize()
        {
            lastRotation = transform.localRotation;
        }

        public void RotateTurret()
        {
            if (turret.localRotation != lastRotation)
            {
                turret.localRotation = lastRotation;
            }
                
            Matrix4x4 parentMatrix = transform.worldToLocalMatrix;
            Vector3 turretDiff = parentMatrix * targetVector;

            Quaternion desiredRotation = Quaternion.LookRotation(turretDiff, transform.up);
            desiredRotation.eulerAngles = new Vector3(0, desiredRotation.eulerAngles.y, 0);

            turret.localRotation = Quaternion.RotateTowards(turret.localRotation, desiredRotation, Time.deltaTime * turretRotationSpeed);
        }

        private void LateUpdate()
        {
            if(vehicleController.IsUpsideDown || turretLock)
            {
                return; 
            }

            RotateTurret();
        }

        private void Update()
        {
            turretLock = inputProvider.TurretLockKey;
            if(!turretLock)
            {
                targetingWorldSpacePosition = mouseActionsProvider.CameraTargetingPosition;
                targetVector = targetingWorldSpacePosition - transform.position;
            }
            lastRotation = turret.localRotation;
        }
    }
}
