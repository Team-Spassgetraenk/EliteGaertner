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
        var profileInfo = _profileDbs.GetPublicProfile(profileId);
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
    
    public void UpdateProfile(PrivateProfileDto dto) //update
    {
        try
        {
            _profileDbs.EditProfile(dto);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Profil konnte nicht mit neuen Informationen aktualisert werden.", ex);
        }
    }

    //Int als Rückgabe, weil beim Registrierungsprozess für Präferenzen und HarvestUpload die ProfileId benötigen
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
        int? profileId;

        //Überprüf, ob eingegebene Credentials korrekt waren
        try
        {
            profileId = _profileDbs.CheckPassword(credentials.EMail, credentials.PasswordHash);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Login konnte technisch nicht durchgeführt werden.", ex);
        }

        //Falls nicht -> Exception werfen
        if (!profileId.HasValue)
            throw new UnauthorizedAccessException("Ungültige Anmeldedaten");

        //Benötigte Profilinformationen von der Datenbank holen
        var profileHarvests = _harvestDbs.GetProfileHarvestUploads(profileId.Value);
        var profileInfo = _profileDbs.GetPrivateProfile(profileId.Value);

        //ProfileInfo um HarvestUploads ergänzen
        return profileInfo with
        {
            HarvestUploads = profileHarvests.ToList(),
        };
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