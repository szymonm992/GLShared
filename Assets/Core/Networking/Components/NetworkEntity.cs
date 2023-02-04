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
        [SerializeField][HideInInspector] private float tickRate = 66f;

        protected float sendRate = 0;
        protected float timeLastSendingPosition;
        protected bool isPlayer = false;
        protected float entityVelocity;

        public NetworkEntityType EntityType => objectType;
        public INetworkTransform CurrentNetworkTransform { get; }
        public float SendRate => sendRate;
        public float TickRate => tickRate;
        public bool IsSender => isSender;
        public float EntityVelocity => entityVelocity;

        protected virtual void Update()
        {
            if (!isSender || sendRate <= 0)
            {
                return;
            }

            if (timeLastSendingPosition >= sendRate)
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
            sendRate = 1f / tickRate;
        }
    }
}
