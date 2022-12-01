using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frontend.Scripts
{
    public static class GameObjectExtensions
    {
       public static void ToggleGameObjectIfActive(this GameObject gameObject, bool value)
        {
            if(gameObject.activeInHierarchy != value)
            {
                gameObject.SetActive(value);
            }
        }
    }
}
