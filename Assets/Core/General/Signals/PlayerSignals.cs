using GLShared.General.Interfaces;
using GLShared.General.Models;
using UnityEngine;

namespace GLShared.General.Signals
{
    public class PlayerSignals
    {
        public class OnPlayerSpawned
        {
            public PlayerProperties PlayerProperties { get; set; }
        }

        public class OnPlayerInitialized
        {
            public PlayerProperties PlayerProperties { get; set; }
            public IPlayerInputProvider InputProvider { get; set; }
            public VehicleStatsBase VehicleStats { get; set; }
        }

        public class OnAllPlayersInputLockUpdate
        {
            public bool LockPlayersInput { get; set; }
        }

        public class OnPlayerDetectionStatusUpdate
        {
            public string Username { get; set; }
            public bool SpottedStatus { get; set; }
        }

        public class OnPlayerShot
        {
            public string Username { get; set; }
            public string ShellId { get; set; }
            public Vector3 ShellSpawnPosition { get; set; }
            public Vector3 ShellSpawnEulerAngles { get; set; }
            public (Vector3, float) TargetingProperties { get; set; }
        }

        public class OnBattleTimeChanged
        {
            public int CurrentMinutesLeft { get; set; }
            public int CurrentSecondsLeft { get; set; }
        }
    }
}


