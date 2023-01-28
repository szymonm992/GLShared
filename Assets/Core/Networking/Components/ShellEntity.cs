using GLShared.General.Interfaces;
using GLShared.General.Models;
using GLShared.Networking.Interfaces;
using GLShared.Networking.Models;
using Zenject;

namespace GLShared.Networking.Components
{
    public class ShellEntity : NetworkEntity
    {
        [Inject] private readonly IShellController shellController;
        [Inject(Optional = true)] private readonly ISyncInterpolator syncInterpolator;

        private ShellProperties shellProperties;

        public ShellProperties Properties => shellProperties;
        public string Username => currentNetworkTransform.Username;

        [Inject]
        public void Construct(ShellProperties propertiesAtPrefab)
        {
            propertiesAtPrefab.ShellContext = shellProperties.ShellContext;
            UpdateProperties(propertiesAtPrefab);

            currentNetworkTransform = new()
            {
                Position = transform.position,
                EulerAngles = transform.eulerAngles,
                GunAngleX = 0,
                TurretAngleY = 0,
                TimeStamp = 0d,
                CurrentSpeed = EntityVelocity,
                Username = shellProperties.ShellId,
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
            currentNetworkTransform.Update(transform, EntityVelocity);
            syncManager.SyncShell(shellController);
        }

        public override void ReceiveSyncPosition(NetworkTransform newNetworkTransform)
        {
            if (isSender)
            {
                return;
            }

            currentNetworkTransform = newNetworkTransform;
            base.ReceiveSyncPosition(currentNetworkTransform);
            syncInterpolator.ProcessCurrentNetworkTransform(currentNetworkTransform);
        }
    }
}
