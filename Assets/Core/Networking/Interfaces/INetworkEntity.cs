using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLShared.Networking.Enums;

namespace GLShared.Networking.Interfaces
{
    public interface INetworkEntity
    {
        public NetworkEntityType EntityType { get; }
    }
}
