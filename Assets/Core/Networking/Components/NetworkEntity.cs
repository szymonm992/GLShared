using UnityEngine;
using UnityEditor;
using GLShared.Networking.Enums;
using GLShared.Networking.Interfaces;
using Zenject;
using GLShared.General.Interfaces;
using GLShared.Networking.Models;

namespace GLShared.Networking.Components
{
    
    public class NetworkEntity : MonoBehaviour, INetworkEntity
    {
        protected const string NETWORK_ENTITY_DEFAULT_VALUE = "NETWORK_ENTITY";

        [Inject] protected readonly ISyncManager syncManager;
        [Inject] protected readonly SignalBus signalBus;

        [SerializeField] protected NetworkEntityType objectType;
        [SerializeField] protected bool isSender = false;

        protected float timeLastSendingPosition;
        protected bool isPlayer = false;
        protected float entityVelocity;
        protected float tickRate;

        public NetworkEntityType EntityType => objectType;
        public INetworkTransform CurrentNetworkTransform { get; }
        public bool IsSender => isSender;
        public float EntityVelocity => entityVelocity;

        public virtual float DefaultTickRate => 20.0f;
        public float SendRate => 1.0f / tickRate;
        public float TickRate => tickRate;

        protected virtual void Update()
        {
            if (!isSender || SendRate <= 0)
            {
                return;
            }

            if (timeLastSendingPosition >= SendRate)
            {
                SendSyncPosition();
                timeLastSendingPosition = 0;
                return;
            }

            timeLastSendingPosition += Time.deltaTime;
        }

        public virtual void SendSyncPosition()
        {
        }

        public virtual void ReceiveSyncPosition(INetworkTransform newNetworkTransform)
        {
        }

        public virtual void Initialize()
        {
            tickRate = DefaultTickRate;
        }
    }
}
