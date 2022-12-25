using UnityEngine;
using GLShared.Networking.Enums;
using GLShared.Networking.Interfaces;
using Zenject;
using GLShared.General.Interfaces;
using GLShared.Networking.Models;

namespace GLShared.Networking.Components
{
    public class NetworkEntity : MonoBehaviour, INetworkEntity, IInitializable
    {
        [Inject] protected readonly ISyncManager syncManager;
        [Inject] protected readonly SignalBus signalBus;

        [SerializeField] private NetworkEntityType objectType;
        [SerializeField] private float syncRate = 0.2f;
        [SerializeField] private bool isSender = false;

        protected NetworkTransform currentNetworkTransform;
        protected float currentSyncTimer = 0;
        protected bool isPlayer = false;
        public NetworkEntityType EntityType => objectType;
        public NetworkTransform CurrentNetworkTransform => currentNetworkTransform;
        public float SyncRate => syncRate;
        public bool IsPlayer => isPlayer;
        public virtual float EntityVelocity => 0;

        protected void Update()
        {
            if(!isSender || syncRate <= 0)
            {
                return;
            }

            if(currentSyncTimer < syncRate)
            {
                currentSyncTimer += Time.deltaTime;
            }
            else
            {
                SyncPosition();
                currentSyncTimer = 0;
            }
        }

        public virtual void SyncPosition()
        {
            if (currentNetworkTransform.HasChanged(transform, 0.001f))
            {
                currentNetworkTransform.Update(transform, EntityVelocity);
                syncManager.SyncPosition(this);
            }
        }

        public virtual void Initialize()
        {
            isPlayer = (this is PlayerEntity);

            if (currentNetworkTransform == null)
            {
                currentNetworkTransform = new()
                {
                    Position = transform.position,
                    EulerAngles = transform.eulerAngles,
                    TimeStamp = 0d,
                    CurrentSpeed = EntityVelocity,
                    Username = "OBSTACLE",
                };
            }
        }
    }
}
