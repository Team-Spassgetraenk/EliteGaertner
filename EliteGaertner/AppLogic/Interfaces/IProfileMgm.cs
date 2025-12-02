using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;

public interface IProfileMgm
{
    ProfileDto GetProfile(int userId);

    bool UpdateProfile(ProfileDto profile);

    bool SetContactVisibility(int userId, ContactVisibilityDto visibility);
}