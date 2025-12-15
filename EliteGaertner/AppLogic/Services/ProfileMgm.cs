using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;

namespace AppLogic.Services;

public class ProfileMgm : IProfileMgm
{
    public ProfileDto GetProfile(int userId)
    {
        throw new NotImplementedException();
    }

    public bool UpdateProfile(ProfileDto profile)
    {
        Console.WriteLine("Profildaten geändert");
        return true;
    }

    public bool SetContactVisibility(ContactVisibilityDto dto)
    {
        Console.WriteLine("Kontaktdaten geändert");
        return true;
    }
}