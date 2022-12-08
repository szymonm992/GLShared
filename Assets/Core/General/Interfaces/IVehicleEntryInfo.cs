using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface IVehicleEntryInfo 
    {
        string VehicleName { get; }
        GameObject VehiclePrefab { get; }
    }
}
