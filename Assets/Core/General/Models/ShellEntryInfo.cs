using GLShared.General.Interfaces;
using UnityEngine;
using Zenject;

namespace GLShared.General.Models
{
    [System.Serializable]
    public class ShellEntryInfo : IShellEntryInfo
    {
        [SerializeField] private string shellId;
        [SerializeField] private string shellName;
        [SerializeField] private GameObjectContext shellPrefab;

        public string ShellId => shellId;
        public string ShellName => shellName;
        public GameObjectContext ShellPrefab => shellPrefab;
    }
}