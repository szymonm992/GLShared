using UnityEngine;

namespace GLShared.General.Interfaces
{
    public interface IRNGHandler
    {
        float RandomGaussian(float minimumValue = 0f, float maximumValue = 1f);
    }
}
