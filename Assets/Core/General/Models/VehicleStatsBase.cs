using GLShared.General.Interfaces;
using UnityEngine;

namespace GLShared.General.Models
{
    public abstract class VehicleStatsBase : MonoBehaviour, IVehicleStats
    {
        [Header("General")]
        [SerializeField] protected string vehicleName;

        [Header("Movement")]
        [SerializeField] protected float mass;
        [SerializeField] protected float drag;
        [SerializeField] protected float angularDrag;
        [SerializeField] protected float steerForce;

        [Header("Combat")]
        [SerializeField] protected float turretRotationSpeed;
        [SerializeField] protected float gunRotationSpeed;
        [SerializeField] protected float gunDepression;
        [SerializeField] protected float gunElevation;
        [SerializeField] protected bool stabilizeGun = true;
        [SerializeField] protected bool stabilizeTurret = true;

        public string VehicleName => vehicleName;

        #region MOVEMENT PARAMETERS
        //general
        public float Mass => mass;
        public float Drag => drag;
        public float AngularDrag => angularDrag;
        public float SteerForce => steerForce;
        #endregion


        #region COMBAT
        public float TurretRotationSpeed => turretRotationSpeed;
        public float GunRotationSpeed => gunRotationSpeed;
        public float GunDepression => gunDepression;
        public float GunElevation => gunElevation;
        public bool StabilizeGun => stabilizeGun;
        public bool StabilizeTurret => stabilizeTurret;
        #endregion
    }
}
