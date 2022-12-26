namespace GLShared.Networking.Models
{
    public class NetworkTurretTransform
    {
        public string Username { get; set; }
        public bool NeedsUpdate { get; set; }
        public float GunAnglesX 
        {
            get => GunAnglesX;
            set
            {
                this.GunAnglesX = value;
                NeedsUpdate = true;
            }
        }
        public float TurretAnglesY
        {
            get => TurretAnglesY;
            set
            {
                this.TurretAnglesY = value;
                NeedsUpdate = true;
            }
        }
    }
}
