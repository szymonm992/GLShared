using GLShared.General.Enums;

namespace GLShared.General.Interfaces
{
    public interface IArmor 
    {
        ArmorType ArmorType { get; }

        float Thickness { get; }
    }
}
