using GLShared.General.Models;
using GLShared.Networking.Models;
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

        public static ISFSObject ToISFSOBject(this NetworkTransform transform)
        {
            ISFSObject data = new SFSObject();

            data.PutUtfString("username", transform.Username);
            data.PutDouble("posX", transform.Position.x);
            data.PutDouble("posY", transform.Position.y);
            data.PutDouble("posZ", transform.Position.z);

            data.PutDouble("rotAnglesX", transform.EulerAngles.x);
            data.PutDouble("rotAnglesY", transform.EulerAngles.y);
            data.PutDouble("rotAnglesZ", transform.EulerAngles.z);
            return data;
        }
    }
}
