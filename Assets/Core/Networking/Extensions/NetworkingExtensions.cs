using GLShared.General.Models;
using Sfs2X.Entities.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.Networking.Extensions
{
    public static class NetworkingExtensions
    {
       public static ISFSObject ToISFSOBject(this PlayerProperties properties)
        {
            ISFSObject data = new SFSObject();
            data.PutUtfString("username", properties.User.Name);
            data.PutFloat("spawnPositionX", properties.SpawnPosition.x);
            data.PutFloat("spawnPositionY", properties.SpawnPosition.y);
            data.PutFloat("spawnPositionZ", properties.SpawnPosition.z);
            data.PutFloat("spawnRotationX", properties.SpawnRotation.eulerAngles.x);
            data.PutFloat("spawnRotationY", properties.SpawnRotation.eulerAngles.y);
            data.PutFloat("spawnRotationZ", properties.SpawnRotation.eulerAngles.z);
            return data;
        }

    }
}
