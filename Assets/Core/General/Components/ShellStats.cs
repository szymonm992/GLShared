using GLShared.General.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Components
{
    public class ShellStats : MonoBehaviour, IShellStats
    {
        [SerializeField] private float speed;
        [SerializeField] private float gravityMultiplier;

        [SerializeField] private float penetration;
        [SerializeField] private float damage;
        [SerializeField] private float caliber;

        public float Speed => speed;
        public float GravityMultiplier => gravityMultiplier;

        public float Penetration => penetration;
        public float Damage => damage;
        public float Caliber => caliber;
    }
}
