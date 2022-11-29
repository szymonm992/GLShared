using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface IMouseActionsProvider
    {
        Vector3 CameraTargetingPosition { get; }
    }
}
