using UnityEngine;

namespace GLShared.General.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameParameters", menuName = "UT/Game settings/GameParameters")]
    public class GameParameters : ScriptableObject
    {
        [Header("General parameters")][Range(3f, 5f)]
        [SerializeField] private float speedMultiplier = 4f;

        [Header("Vehicles wheel parameters")]
        [SerializeField] private float maxWheelDetectionAngle = 75f;
        [SerializeField] private float maxWheelDrivingAngle = 45f;

        [Header("Vehicles air control")]
        [SerializeField] private float airControlAngleThreshold = 15f;
        [SerializeField] private float airControlForce = 5f;

        [Header("Aiming")]
        [SerializeField] private float gunMaxAimingDistance = 3000f;

        [Header("Network sync")]
        [SerializeField] private float serverSettingsUpdateRate = 15f;

        public float SpeedMultiplier => speedMultiplier;

        public float MaxWheelDetectionAngle => maxWheelDetectionAngle;
        public float MaxWheelDrivingAngle => maxWheelDrivingAngle;

        public float AirControlAngleThreshold => airControlAngleThreshold;
        public float AirControlForce => airControlForce;

        public float GunMaxAimingDistance => gunMaxAimingDistance;

        public float ServerSettingsUpdateRate => serverSettingsUpdateRate;
    }
}
