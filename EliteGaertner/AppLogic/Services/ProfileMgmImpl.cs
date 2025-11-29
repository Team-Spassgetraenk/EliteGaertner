using AppLogic.Logic.Data_Transfer_Objects;
using AppLogic.Logic.Interfaces;

namespace AppLogic.Logic.Services;

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