using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface ISyncManager
    {
        void CreatePlayer(bool isLocal, string vehicleName, Vector3 spawnPosition, Quaternion spawnRotation);
    }
}
