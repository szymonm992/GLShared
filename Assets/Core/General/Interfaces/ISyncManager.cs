using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface ISyncManager
    {
        int SpawnedPlayersAmount { get; }

        void CreatePlayer(bool isLocal, string vehicleName, Vector3 spawnPosition, Quaternion spawnRotation);
    }
}
