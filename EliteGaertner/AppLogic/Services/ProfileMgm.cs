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
    
    public PrivateProfileDto GetPrivProfile(int profileId) 
    {
        //Da wir die Datenbankzugriffe auf verschiedene Klassen aufgesplittet haben, müssen wir 
        //hier zwei verschiedene Datenbankzugriffe durchführen (Profilinfos, Harvestuploads)
        var profileHarvests = _harvestDbs.GetProfileHarvestUploads(profileId);
        var profileInfo = _profileDbs.GetPrivateProfile(profileId);
        var privateProfile = profileInfo with
        {
            HarvestUploads = profileHarvests.ToList(),
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

    public int RegisterProfile(PrivateProfileDto newProfile, CredentialProfileDto credentials)
    {
        try
        {
            return _profileDbs.SetNewProfile(newProfile, credentials);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Registrierung konnte nicht durchgeführt werden.", ex);
        }
    }

    public PrivateProfileDto LoginProfile(CredentialProfileDto credentials) 
    {
        int? profileId = _profileDbs.CheckPassword(credentials.EMail, credentials.PasswordHash);
        
        if (profileId.HasValue)
        {
            //Da wir die Datenbankzugriffe auf verschiedene Klassen aufgesplittet haben, müssen wir 
            //hier zwei verschiedene Datenbankzugriffe durchführen (Profilinfos, Harvestuploads)
            var profileHarvests = _harvestDbs.GetProfileHarvestUploads(profileId.Value);
            var profileInfo = _profileDbs.GetPrivateProfile(profileId.Value);
            var dto = profileInfo with 
            {
                HarvestUploads = profileHarvests.ToList(),
            };
            
            return dto;
        }
        
        throw new UnauthorizedAccessException("Ungültige Anmeldedaten");
    }

    public void UpdateCredentials(CredentialProfileDto credentials)
    {
        try
        {
            _profileDbs.EditPassword(credentials);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Anmeldedaten konnten nicht aktualisiert werden.", ex);
        }
    }

    public List<PreferenceDto> GetPreference(int profileId)
    {
        return _profileDbs.GetUserPreference(profileId).ToList();
    }

    public void SetPreference(List<PreferenceDto> preferences)
    {
        try
        {
            _profileDbs.SetUserPreference(preferences);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Präferenzen konnten nicht gespeichert werden.", ex);
        }
    }
}