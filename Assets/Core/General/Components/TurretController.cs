using GLShared.General.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEngine.GraphicsBuffer;

namespace GLShared.General.Components
{
    public class TurretController : MonoBehaviour, IInitializable, ITurretController
    {
        [Inject] private readonly IVehicleController vehicleController;

        [SerializeField] private float turretRotationSpeed = 48f;
        [SerializeField] private Transform turret;
        
        private bool turretLock;
        private Vector3 targetingWorldSpacePosition;
        private Vector3 targetVector;
        private Quaternion lastRotation;
        public Vector3 TargetingWorldSpacePosition
        { 
            get => targetingWorldSpacePosition;
            set => targetingWorldSpacePosition = value;
        }

        public bool TurretLock
        {
            get => turretLock; 
            set => this.turretLock = value;
        }

        public void Initialize()
        {
            lastRotation = transform.localRotation;
        }

        public void RotateTurret()
        {
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
            targetVector = targetingWorldSpacePosition - transform.position;
            lastRotation = turret.localRotation;
        }
    }
}
