using GLShared.General.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Utilities
{
    public static class PlayerInputExtensions
    {
        public static PlayerInput Empty(this PlayerInput input)
        {
            return new PlayerInput(0, 0, false, true, Vector3.zero);
        }
    }
}
