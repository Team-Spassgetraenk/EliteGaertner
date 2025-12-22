using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;

namespace AppLogic.Services;

public class ProfileMgm : IProfileMgm
{

    public PrivateProfileDto VisitReceiverProfile(int userId)
    {
        throw new NotImplementedException();
    }

    public PrivateProfileDto VisitCreatorProfile(int userId)
    {
        throw new NotImplementedException();
    }

    public bool UpdateProfile(PrivateProfileDto profile)
    {
        Console.WriteLine("Profildaten geändert");
        return true;
    }

    public bool SetContactVisibility(ContactVisibilityDto dto)
    {
        Console.WriteLine("Kontaktdaten geändert");
        return true;
    }

    public PrivateProfileDto RegisterProfile(PrivateProfileDto newProfile)
    {
        throw new NotImplementedException();
    }

    public PrivateProfileDto LoginProfile(PrivateProfileDto receiverProfile)
    {
        throw new NotImplementedException();
    }

    public List<PreferenceDto> SetPreferences(PreferenceDto profilePreferences)
    {
        throw new NotImplementedException();
    }
}