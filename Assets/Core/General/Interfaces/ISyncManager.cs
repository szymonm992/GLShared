using Sfs2X.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GLShared.General.Interfaces
{
    public interface ISyncManager : IInitializable
    {
        double CurrentServerTime { get; }
        int SpawnedPlayersAmount { get; }

        void TryCreatePlayer(User user);
    }
}
