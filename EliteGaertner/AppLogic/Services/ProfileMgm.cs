using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using DataManagement;
using DataManagement.Interfaces;

namespace AppLogic.Services;

public class ProfileMgm : IProfileMgm
{
    private readonly IProfileDbs _profileDbs;
    private readonly IHarvestDbs _harvestDbs;

    public ProfileMgm(IProfileDbs profileDbs, IHarvestDbs harvestDbs)
    {
        _profileDbs = profileDbs;
        _harvestDbs = harvestDbs;
    }

    public bool CheckUsernameExists(string username)
    {
        return _profileDbs.CheckUsernameExists(username);
    }
    
    public PublicProfileDto VisitPublicProfile(int profileId) //get
    {
        //Da wir die Datenbankzugriffe auf verschiedene Klassen aufgesplittet haben, müssen wir 
        //hier zwei verschiedene Datenbankzugriffe durchführen (Profilinfos, Harvestuploads)
        var profileHarvests = _harvestDbs.GetProfileHarvestUploads(profileId);
        var profileInfo = _profileDbs.GetPrivateProfile(profileId);
        var publicProfile = new PublicProfileDto
        {
            ProfileId = profileInfo.ProfileId,
            ProfilepictureUrl = profileInfo.ProfilepictureUrl,
            UserName = profileInfo.UserName,
            Profiletext = profileInfo.Profiletext,
            UserCreated = profileInfo.UserCreated,
            HarvestUploads = profileHarvests.ToList(),
        };
            
        return publicProfile;
    }

    public PrivateProfileDto VisitPrivateProfile(int profileId) // get
    {
        //Da wir die Datenbankzugriffe auf verschiedene Klassen aufgesplittet haben, müssen wir 
        //hier zwei verschiedene Datenbankzugriffe durchführen (Profilinfos, Harvestuploads)
        var profileHarvests = _harvestDbs.GetProfileHarvestUploads(profileId);
        var profileInfo = _profileDbs.GetPrivateProfile(profileId);
        var privateProfile = new PrivateProfileDto
        {
            ProfileId = profileInfo.ProfileId,
            ProfilepictureUrl = profileInfo.ProfilepictureUrl,
            UserName = profileInfo.UserName,
            FirstName = profileInfo.FirstName,
            LastName = profileInfo.LastName,
            EMail = profileInfo.EMail,
            Phonenumber = profileInfo.Phonenumber,
            Profiletext = profileInfo.Profiletext,
            ShareMail = profileInfo.ShareMail,
            SharePhoneNumber = profileInfo.SharePhoneNumber,
            UserCreated = profileInfo.UserCreated,
            HarvestUploads = profileHarvests.ToList(),
            PreferenceDtos = profileInfo.PreferenceDtos
        };
            
        return privateProfile;
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
            //Da wir die Datenbankzugriffe auf verschiedene Klassen aufgesplittet haben, müssen wir 
            //hier zwei verschiedene Datenbankzugriffe durchführen (Profilinfos, Harvestuploads)
            var profileHarvests = _harvestDbs.GetProfileHarvestUploads(profileId.Value);
            var profileInfo = _profileDbs.GetPrivateProfile(profileId.Value);
            var dto = new PrivateProfileDto
            {
                ProfileId = profileInfo.ProfileId,
                ProfilepictureUrl = profileInfo.ProfilepictureUrl,
                UserName = profileInfo.UserName,
                FirstName = profileInfo.FirstName,
                LastName = profileInfo.LastName,
                EMail = profileInfo.EMail,
                Phonenumber = profileInfo.Phonenumber,
                Profiletext = profileInfo.Profiletext,
                ShareMail = profileInfo.ShareMail,
                SharePhoneNumber = profileInfo.SharePhoneNumber,
                UserCreated = profileInfo.UserCreated,
                HarvestUploads = profileHarvests.ToList(),
                PreferenceDtos = profileInfo.PreferenceDtos
            };
            
            return dto;
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