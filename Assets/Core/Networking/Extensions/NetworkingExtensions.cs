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
            data.PutFloat("posX", transform.Position.x);
            data.PutFloat("posY", transform.Position.y);
            data.PutFloat("posZ", transform.Position.z);

            data.PutFloat("rotAnglesX", transform.EulerAngles.x);
            data.PutFloat("rotAnglesY", transform.EulerAngles.y);
            data.PutFloat("rotAnglesZ", transform.EulerAngles.z);
            return data;
        }

        public static NetworkTransform ToNetworkTransform(this ISFSObject data)
        {
            NetworkTransform transform = new()
            {
                Position = new Vector3(data.GetFloat("posX"), data.GetFloat("posY"), data.GetFloat("posZ")),
                EulerAngles = new Vector3(data.GetFloat("rotAnglesX"), data.GetFloat("rotAnglesY"), data.GetFloat("rotAnglesZ")),
                Username = data.GetUtfString("username"),
                TimeStamp = data.GetLong("timeStamp"),
                CurrentSpeed = 0,
            };

            return transform;
        }
    }
}
