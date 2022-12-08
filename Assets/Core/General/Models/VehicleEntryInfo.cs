using GLShared.General.Interfaces;
using UnityEngine;

namespace GLShared.General.Models
{
    [System.Serializable]
    public class VehicleEntryInfo : IVehicleEntryInfo
    {
        [SerializeField] private string vehicleName;
        [SerializeField] private GameObject vehiclePrefab;

        public string VehicleName => vehicleName;
        public GameObject VehiclePrefab => vehiclePrefab;
    }
}
