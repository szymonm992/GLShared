using GLShared.General.Enums;
using GLShared.General.Interfaces;
using GLShared.Networking.Components;
using UnityEngine;
using Zenject;

namespace GLShared.General.Components
{
    public class Armor : MonoBehaviour, IArmor
    {
        [Inject] private PlayerEntity playerEntity;

        [SerializeField] private ArmorType armorType;
        [SerializeField] private float thickness = 10f;

        public ArmorType ArmorType => armorType;
        public float Thickness => thickness;
        public string OwnerUsername => playerEntity.Username;

    }
}
