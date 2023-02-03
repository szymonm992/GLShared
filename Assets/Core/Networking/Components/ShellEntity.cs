using GLShared.General.Interfaces;
using GLShared.General.Models;
using GLShared.General.Signals;
using GLShared.Networking.Interfaces;
using GLShared.Networking.Models;
using Zenject;

namespace GLShared.Networking.Components
{
    public class ShellEntity : NetworkEntity
    {
        private const string SHELL_DEFAULT_ENTITY_NAME = "NETWORK_ENTITY";
        [Inject] private readonly IShellController shellController;
        [Inject] private readonly GameObjectContext context;
        [Inject(Optional = true)] private readonly ISyncInterpolator syncInterpolator;

        private ShellProperties shellProperties;
        private NetworkShellTransform currentNetworkTransform;

        public ShellProperties Properties => shellProperties;
        public string Username => currentNetworkTransform.Identifier;
        public NetworkShellTransform CurrentTransform => currentNetworkTransform;

        [Inject]
        public void Construct(ShellProperties propertiesAtPrefab)
        {
            propertiesAtPrefab.ShellContext = context;
            UpdateProperties(propertiesAtPrefab);

            currentNetworkTransform = new NetworkShellTransform()
            {
                Position = transform.position,
                EulerAngles = transform.eulerAngles,
                TimeStamp = 0d,
                CurrentSpeed = EntityVelocity,
                Identifier = shellProperties.ShellSceneIdentifier.ToString(),
            };
        }

        public void UpdateProperties(ShellProperties properties)
        {
            shellProperties = properties;
            transform.SetPositionAndRotation(shellProperties.SpawnPosition, shellProperties.SpawnRotation);
        }

        public override void SendSyncPosition()
        {
            base.SendSyncPosition();

            entityVelocity = shellController.Velocity;
            currentNetworkTransform.Update(transform, EntityVelocity);
            syncManager.SyncShell(this);
        }

        public override void ReceiveSyncPosition(INetworkTransform newNetworkTransform)
        {
            if (isSender)
            {
                return;
            }

            currentNetworkTransform = (NetworkShellTransform)newNetworkTransform;
            base.ReceiveSyncPosition(currentNetworkTransform);
            syncInterpolator.ProcessCurrentNetworkTransform(currentNetworkTransform);
        }

        public override void Initialize()
        {
            base.Initialize();

            if (currentNetworkTransform == null)
            {
                currentNetworkTransform = new NetworkShellTransform()
                {
                    Position = transform.position,
                    EulerAngles = transform.eulerAngles,
                    TimeStamp = 0d,
                    CurrentSpeed = EntityVelocity,
                    Identifier = SHELL_DEFAULT_ENTITY_NAME,
                };
            }
        }
    }
}
