using GLShared.General.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface IShellsDatabase
    {
        IEnumerable<ShellEntryInfo> AllShells { get; }
        abstract ShellEntryInfo GetShellInfo(string id);
    }
}
