using GLShared.General.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface IVehiclesDatabase 
    {
        IEnumerable<VehicleEntryInfo> AllVehicles { get; }
        abstract VehicleEntryInfo GetVehicleInfo(string name);

    }
}
