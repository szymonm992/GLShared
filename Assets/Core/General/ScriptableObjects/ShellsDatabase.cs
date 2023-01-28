using GLShared.General.Interfaces;
using GLShared.General.Models;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.ScriptableObjects
{
    public class ShellsDatabase : ScriptableObject, IShellsDatabase
    {
        public virtual IEnumerable<ShellEntryInfo> AllShells { get; }

        public ShellEntryInfo GetShellInfo(string shellId)
        {
            foreach (var info in AllShells)
            {
                if (info.ShellId == shellId)
                {
                    return info;
                }
            }

            return null;
        }
    }
}
