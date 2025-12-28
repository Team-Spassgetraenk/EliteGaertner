using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;

namespace AppLogic.Services;

public class ProfileMgm : IProfileMgm
{
    private readonly IProfileDbs _profileDbs;

    public ProfileMgm(IProfileDbs profileDbs)
    {
        _profileDbs = profileDbs;
    }

    public bool CheckUsernameExists(string username)
    {
        return _profileDbs.CheckUsernameExists(username);
    }
    
    public PublicProfileDto VisitReceiverProfile(int userId) //get
    {
        return _profileDbs.GetPublicProfile(userId);
    }

    public PrivateProfileDto VisitCreatorProfile(int userId) // get
    {
        return _profileDbs.GetPrivateProfile(userId);
    }

    public bool UpdateProfile(PrivateProfileDto dto) //update
    {
        var privateProfile = _profileDbs.EditProfile(dto);

        if (privateProfile.ProfileId <= 0)
        {
            Console.WriteLine("Profildaten konnten nicht geändert werden");
            return false;
        }
        
        Console.WriteLine("Profildaten geändert");
        return true;
    }

    public bool UpdateContactVisibility(ContactVisibilityDto dto) //update Visibility
    {
        if (_profileDbs.UpdateContactVisibility(dto))
        { 
            Console.WriteLine("Kontaktdaten geändert");
            return true;
        }
        
        Console.WriteLine("Kontaktdaten konnten nicht geändert werden");
        return false;
    }

    public PrivateProfileDto RegisterProfile(PrivateProfileDto newProfile)
    {
        
        return _profileDbs.SetNewProfile(newProfile);
        
        // HarvestUploads und PreferenceDtos sind null - wie in der Doku beschrieben
        // Hier können ggf. Default-Werte gesetzt werden
    }

    public PrivateProfileDto LoginProfile(PrivateProfileDto receiverProfile) //TODO Nicolas über Auth
    {
        int? profileId = _profileDbs.CheckPassword(receiverProfile.EMail, receiverProfile.PasswordHash);
        
        if (profileId.HasValue)
        {
            return _profileDbs.GetPrivateProfile(profileId.Value);
        }
        
        throw new UnauthorizedAccessException("Ungültige Anmeldedaten");
    }

    public List<PreferenceDto> GetPreference(int profileId)
    {
        return _profileDbs.GetUserPreference(profileId).ToList();
    }

    public bool SetPreference(List<PreferenceDto> preferences)
    {
        return _profileDbs.SetUserPreference(preferences);
    }
}