using GLShared.Networking.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.Networking.Interfaces
{
    public interface ISyncInterpolator
    {
        void ProcessCurrentNetworkTransform(NetworkTransform nTransform);
    }
}
