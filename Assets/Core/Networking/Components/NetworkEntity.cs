using UnityEngine;
using GLShared.Networking.Enums;
using GLShared.Networking.Interfaces;

namespace GLShared.Networking.Components
{
    public class NetworkEntity : MonoBehaviour, INetworkEntity
    {
        [SerializeField] private NetworkEntityType objectType;
        public NetworkEntityType EntityType => objectType;
    }
}
