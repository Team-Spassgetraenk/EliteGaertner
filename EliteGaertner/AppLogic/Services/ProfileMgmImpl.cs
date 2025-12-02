using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;

namespace AppLogic.Services;

public class ProfileMgmImpl : IProfileMgm
{
    public ProfileDto GetProfile(int userId)
    {
        throw new NotImplementedException();
    }

    public bool UpdateProfile(ProfileDto profile)
    {
        throw new NotImplementedException();
    }

    public bool SetContactVisibility(int userId, ContactVisibilityDto visibility)
    {
        throw new NotImplementedException();
    }
}