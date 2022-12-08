using UnityEngine;
using GLShared.Networking.Enums;
using GLShared.Networking.Interfaces;
using Unity.VisualScripting;

namespace GLShared.Networking.Components
{
    public class NetworkEntity : MonoBehaviour, INetworkEntity
    {
        [SerializeField] private NetworkEntityType objectType;
        public NetworkEntityType EntityType => objectType;
    }
}
