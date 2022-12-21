namespace GLShared.General.Models
{
    public class PlayerInput
    {
        public float Horizontal { get; }
        public float Vertical { get; }
        public bool Brake { get; }
        public bool TurretLockKey { get; }

        public PlayerInput(float horizontal, float vertical, bool brake, bool turretLockKey)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            Brake = brake;
            TurretLockKey = turretLockKey;
        }
    }
}
