using GLShared.General.Enums;
using GLShared.General.Interfaces;
using GLShared.General.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public abstract class UTAxleBase : MonoBehaviour, IInitializable, IVehicleAxle
    {
        public const float SUSPENSION_VISUALS_MOVEMENT_SPEED = 50F;

        [Inject(Id = "mainRig")] protected readonly Rigidbody rig;
        [Inject] protected readonly IVehicleController controller;
        [Inject] protected readonly IPlayerInputProvider inputProvider;


        [SerializeField] protected bool canDrive;
        [SerializeField] protected bool canSteer;
        [SerializeField] protected bool invertSteer = false;

        public UTAxleDebug debugSettings = new UTAxleDebug()
        {
            DrawGizmos = true,
            DrawAxleCenter = true,
            DrawAxlePipes = true,
            DrawMode = UTDebugMode.All
        };

        
        protected IEnumerable<IPhysicsWheel> allWheels;
        protected IEnumerable<IPhysicsWheel> groundedWheels;
        protected bool hasAnyWheel = false;
        protected bool isAxleGrounded;

        public abstract IEnumerable<UTAxlePairBase> WheelPairs { get; }
        public IEnumerable<IPhysicsWheel> AllWheels => allWheels;
        public IEnumerable<IPhysicsWheel> GroundedWheels => groundedWheels;
        public bool CanDrive => canDrive;
        public bool CanSteer => canSteer;
        public bool InvertSteer => invertSteer;
        public bool HasAnyWheelPair => WheelPairs.Any();
        public bool HasAnyWheel => hasAnyWheel;
        public bool IsAxleGrounded => isAxleGrounded;

        protected IEnumerable<IPhysicsWheel> GetGroundedWheels()
        {
            return allWheels.Where(wheel => wheel.IsGrounded == true);
        }

        public abstract IEnumerable<IPhysicsWheel> GetAllWheelsOfAxis(DriveAxisSite axis);

        public virtual void Initialize()
        {
            allWheels = WheelPairs.Select(pair => pair.Wheel).ToArray();
            hasAnyWheel = allWheels.Any();

            if (HasAnyWheelPair)
            {
                foreach (var pair in WheelPairs)
                {
                    pair.Initialize(this);
                }
            }

            groundedWheels = GetGroundedWheels();
            isAxleGrounded = CheckAxleGrounded();
        }

        public virtual void SetSteerAngle(float angleLeft, float angleRight)
        {
        }

        protected bool CheckAxleGrounded()
        {
            return groundedWheels.Count() == allWheels.Count();
        }
    }
}
