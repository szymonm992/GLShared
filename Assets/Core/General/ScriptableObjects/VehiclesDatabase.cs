using GLShared.General.Interfaces;
using GLShared.General.Models;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.ScriptableObjects
{
    public abstract class VehiclesDatabase : ScriptableObject, IVehiclesDatabase
    {
        public virtual IEnumerable<VehicleEntryInfo> AllVehicles { get; }

        public VehicleEntryInfo GetVehicleInfo(string name)
        {
            foreach(var info in AllVehicles)
            {
                if(info.VehicleName == name)
                {
                    return info;
                }
            }
            return null;
        }
    }
}
