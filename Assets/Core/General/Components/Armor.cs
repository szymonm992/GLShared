using GLShared.General.Enums;
using GLShared.General.Interfaces;
using UnityEngine;

namespace GLShared.General.Components
{
    public class Armor : MonoBehaviour, IArmor
    {
        [SerializeField] private ArmorType armorType;
        [SerializeField] private float thickness = 10f;

        public ArmorType ArmorType => armorType;
        public float Thickness => thickness;
    }
}
