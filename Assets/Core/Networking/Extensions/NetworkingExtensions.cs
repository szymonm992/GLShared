using GLShared.General;
using GLShared.General.Models;
using GLShared.Networking.Models;
using Sfs2X.Entities.Data;
using System;
using UnityEngine;

namespace GLShared.Networking.Extensions
{
    public static class NetworkingExtensions
    {
        #region ISFS Object
        public static ISFSObject ToISFSOBject(this PlayerProperties properties, string id)
        {
            ISFSObject data = new SFSObject();

            data.PutUtfString("username", properties.Username);
            data.PutUtfString("id", id);
            data.PutFloat("spawnPositionX", properties.SpawnPosition.x);
            data.PutFloat("spawnPositionY", properties.SpawnPosition.y);
            data.PutFloat("spawnPositionZ", properties.SpawnPosition.z);
            data.PutFloat("spawnRotationX", properties.SpawnRotation.eulerAngles.x);
            data.PutFloat("spawnRotationY", properties.SpawnRotation.eulerAngles.y);
            data.PutFloat("spawnRotationZ", properties.SpawnRotation.eulerAngles.z);

            return data;
        }

        public static ISFSObject ToISFSOBject(this ShellProperties properties)
        {
            ISFSObject data = new SFSObject();

            data.PutUtfString("owner", properties.Username);
            data.PutUtfString("dbId", properties.DatabaseId);
            data.PutInt("id", properties.ShellSceneIdentifier);

            data.PutFloat("spawnPositionX", properties.SpawnPosition.x);
            data.PutFloat("spawnPositionY", properties.SpawnPosition.y);
            data.PutFloat("spawnPositionZ", properties.SpawnPosition.z);
            data.PutFloat("spawnRotationX", properties.SpawnRotation.eulerAngles.x);
            data.PutFloat("spawnRotationY", properties.SpawnRotation.eulerAngles.y);
            data.PutFloat("spawnRotationZ", properties.SpawnRotation.eulerAngles.z);

            data.PutFloat("targetPosX", properties.TargetingPosition.x);
            data.PutFloat("taregetPosY", properties.TargetingPosition.y);
            data.PutFloat("targetPosZ", properties.TargetingPosition.z);

            return data;
        }

        public static ISFSObject ToISFSOBject(this NetworkTransform transform)
        {
            ISFSObject data = new SFSObject();

            data.PutUtfString("u", transform.Identifier);

            data.PutFloat("pX", transform.Position.x);
            data.PutFloat("pY", transform.Position.y);
            data.PutFloat("pZ", transform.Position.z);

            data.PutFloat("rX", transform.EulerAngles.x);
            data.PutFloat("rY", transform.EulerAngles.y);
            data.PutFloat("rZ", transform.EulerAngles.z);

            data.PutFloat("gX", transform.GunAngleX);
            data.PutFloat("tY", transform.TurretAngleY);

            data.PutFloat("v", transform.CurrentSpeed);

            data.PutLong("tim", Convert.ToInt64(GeneralHelper.GenerateTimestamp()));

            return data;
        }

        public static ISFSObject ToISFSOBject(this NetworkShellTransform transform)
        {
            ISFSObject data = new SFSObject();

            data.PutUtfString("id", transform.Identifier);

            data.PutFloat("pX", transform.Position.x);
            data.PutFloat("pY", transform.Position.y);
            data.PutFloat("pZ", transform.Position.z);

            data.PutFloat("rX", transform.EulerAngles.x);
            data.PutFloat("rY", transform.EulerAngles.y);
            data.PutFloat("rZ", transform.EulerAngles.z);

            data.PutFloat("v", transform.CurrentSpeed);

            data.PutLong("tim", Convert.ToInt64(GeneralHelper.GenerateTimestamp()));

            return data;
        }

        public static ISFSObject ToISFSOBject(this PlayerInput playerInput)
        {
            ISFSObject data = new SFSObject();

            data.PutUtfString("u", playerInput.Username);

            data.PutFloat("hor", playerInput.Horizontal);
            data.PutFloat("ver", playerInput.Vertical);
            data.PutFloat("rVer", playerInput.RawVertical);

            data.PutBool("brk", playerInput.Brake);
            data.PutBool("turLck", playerInput.TurretLockKey);
            data.PutBool("shtK", playerInput.ShootingKey);

            data.PutFloat("camX", playerInput.CameraTargetingPosition.x);
            data.PutFloat("camY", playerInput.CameraTargetingPosition.y);
            data.PutFloat("camZ", playerInput.CameraTargetingPosition.z);

            return data;
        }

        #endregion

        public static NetworkTransform ToNetworkTransform(this ISFSObject data)
        {
            return new()
            {
                Position = new Vector3(data.GetFloat("pX"), data.GetFloat("pY"), data.GetFloat("pZ")),
                EulerAngles = new Vector3(data.GetFloat("rX"), data.GetFloat("rY"), data.GetFloat("rZ")),
                Identifier = data.GetUtfString("u"),
                TimeStamp = Convert.ToDouble(data.GetLong("tim")),
                TurretAngleY = data.GetFloat("tY"),
                GunAngleX = data.GetFloat("gX"),
                CurrentSpeed = data.GetFloat("v"),
            };
        }

        public static NetworkShellTransform ToNetworkShellTransform(this ISFSObject data)
        {
            return new()
            {
                Position = new Vector3(data.GetFloat("pX"), data.GetFloat("pY"), data.GetFloat("pZ")),
                EulerAngles = new Vector3(data.GetFloat("rX"), data.GetFloat("rY"), data.GetFloat("rZ")),
                Identifier = data.GetUtfString("id"),
                TimeStamp = Convert.ToDouble(data.GetLong("tim")),
                CurrentSpeed = data.GetFloat("v"),
            };
        }

        public static PlayerInput ToPlayerInput(this ISFSObject data)
        {
            PlayerInput input = new (data.GetUtfString("u"), data.GetFloat("hor"), data.GetFloat("ver"), data.GetFloat("rVer"),
                data.GetBool("brk"), data.GetBool("turLck"),
                new Vector3(data.GetFloat("camX"), data.GetFloat("camY"), data.GetFloat("camZ")), data.GetBool("shtK"));
            return input;
        }
        
    }
}
