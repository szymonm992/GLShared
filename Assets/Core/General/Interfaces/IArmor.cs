using GLShared.General.Enums;
using Zenject;

namespace GLShared.General.Interfaces
{
    public interface IArmor
    {
        ArmorType ArmorType { get; }

        float Thickness { get; }
        string OwnerUsername { get; }
    }
}
