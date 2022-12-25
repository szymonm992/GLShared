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
        #region ISFS Object
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

            data.PutUtfString("u", transform.Username);
            data.PutFloat("posX", transform.Position.x);
            data.PutFloat("posY", transform.Position.y);
            data.PutFloat("posZ", transform.Position.z);

            data.PutFloat("rotX", transform.EulerAngles.x);
            data.PutFloat("rotY", transform.EulerAngles.y);
            data.PutFloat("rotZ", transform.EulerAngles.z);
            return data;
        }

        public static ISFSObject ToISFSOBject(this PlayerInput playerInput)
        {
            ISFSObject data = new SFSObject();

            data.PutUtfString("username", playerInput.Username);

            data.PutFloat("hor", playerInput.Horizontal);
            data.PutFloat("ver", playerInput.Vertical);

            data.PutBool("brk", playerInput.Brake);
            data.PutBool("turLck", playerInput.TurretLockKey);

            data.PutFloat("camX", playerInput.CameraTargetingPosition.x);
            data.PutFloat("camY", playerInput.CameraTargetingPosition.y);
            data.PutFloat("camZ", playerInput.CameraTargetingPosition.z);

            return data;
        }
        #endregion

        public static NetworkTransform ToNetworkTransform(this ISFSObject data)
        {
            NetworkTransform transform = new()
            {
                Position = new Vector3(data.GetFloat("posX"), data.GetFloat("posY"), data.GetFloat("posZ")),
                EulerAngles = new Vector3(data.GetFloat("rotAnglesX"), data.GetFloat("rotAnglesY"), data.GetFloat("rotAnglesZ")),
                Username = data.GetUtfString("u"),
                TimeStamp = 0, //data.GetLong("timeStamp"),
                CurrentSpeed = 0,
            };

            return transform;
        }

        public static PlayerInput ToPlayerInput(this ISFSObject data)
        {
            PlayerInput input = new (data.GetUtfString("username"), data.GetFloat("hor"), data.GetFloat("ver"), 
                data.GetBool("brk"), data.GetBool("turLck"),
                new Vector3(data.GetFloat("camX"), data.GetFloat("camY"), data.GetFloat("camZ")));
            return input;
        }
    }
}
