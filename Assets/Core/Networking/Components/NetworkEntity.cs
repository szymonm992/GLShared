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
        [SerializeField][HideInInspector] protected float syncRate = 0.015f;
        

        protected float timeLastSendingPosition;
        protected bool isPlayer = false;
        protected float entityVelocity;

        public NetworkEntityType EntityType => objectType;
        public INetworkTransform CurrentNetworkTransform { get; }
        public float SyncRate => syncRate;

        public bool IsSender => isSender;
        public float EntityVelocity => entityVelocity;

        protected virtual void Update()
        {
            if (!isSender || syncRate <= 0)
            {
                return;
            }

            if (timeLastSendingPosition >= syncRate)
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
        }
    }
}
