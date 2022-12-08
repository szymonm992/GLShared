using UnityEngine;
using Zenject;

namespace GLShared.General.Interfaces
{
    public interface IVehicleEntryInfo 
    {
        string VehicleName { get; }
        GameObjectContext VehiclePrefab { get; }
    }
}
