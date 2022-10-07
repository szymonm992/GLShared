using UnityEngine;

namespace GLShared.General.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameParameters", menuName = "UT/Game settings/GameParameters")]
    public class GameParameters : ScriptableObject
    {
        [Header("General parameters")]
        [SerializeField] private float speedMultiplier = 4f;

        [Header("Vehicles wheel parameters")]
        [SerializeField] private float maxWheelDetectionAngle = 75f;

        [Header("Vehicles air control")]
        [SerializeField] private float airControlAngleThreshold = 15f;
        [SerializeField] private float airControlForce = 5f;

        public float SpeedMultiplier => speedMultiplier;

        public float MaxWheelDetectionAngle => maxWheelDetectionAngle;

        public float AirControlAngleThreshold => airControlAngleThreshold;
        public float AirControlForce => airControlForce;
    }
}
