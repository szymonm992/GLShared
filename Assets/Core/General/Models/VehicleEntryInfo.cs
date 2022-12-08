using GLShared.General.Interfaces;
using UnityEngine;
using Zenject;

namespace GLShared.General.Models
{
    [System.Serializable]
    public class VehicleEntryInfo : IVehicleEntryInfo
    {
        [SerializeField] private string vehicleName;
        [SerializeField] private GameObjectContext vehiclePrefab;

        public string VehicleName => vehicleName;
        public GameObjectContext VehiclePrefab => vehiclePrefab;
    }
}
