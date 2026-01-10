using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using DataManagement;
using DataManagement.Interfaces;
using Microsoft.Extensions.Logging;

namespace AppLogic.Services;

public class ProfileMgm : IProfileMgm
{
    private readonly IProfileDbs _profileDbs;
    private readonly IHarvestDbs _harvestDbs;
    private readonly ILogger<ProfileMgm> _logger;

    public ProfileMgm(IProfileDbs profileDbs, IHarvestDbs harvestDbs, ILogger<ProfileMgm> logger)
    {
        _profileDbs = profileDbs;
        _harvestDbs = harvestDbs;
        _logger = logger;
    }

    public bool CheckProfileNameExists(string profileName)
    {
        _logger.LogDebug("CheckProfileNameExists called. profileName={ProfileName}", profileName);
        
        var exists = _profileDbs.CheckProfileNameExists(profileName);
        
        _logger.LogDebug("CheckProfileNameExists result. profileName={ProfileName}, exists={Exists}", profileName, exists);
        
        return exists;
    }
    
    public PublicProfileDto VisitPublicProfile(int profileId) //get
    {
        _logger.LogInformation("VisitPublicProfile called. profileId={ProfileId}", profileId);

        try
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

            _logger.LogDebug("VisitPublicProfile success. profileId={ProfileId}, harvestUploadsCount={HarvestUploadsCount}", profileId, publicProfile.HarvestUploads?.Count ?? 0);

            return publicProfile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "VisitPublicProfile failed. profileId={ProfileId}", profileId);
            throw;
        }
    }
    
    public PrivateProfileDto GetPrivProfile(int profileId) 
    {
        _logger.LogInformation("GetPrivProfile called. profileId={ProfileId}", profileId);

        try
        {
            //Da wir die Datenbankzugriffe auf verschiedene Klassen aufgesplittet haben, müssen wir 
            //hier zwei verschiedene Datenbankzugriffe durchführen (Profilinfos, Harvestuploads)
            var profileHarvests = _harvestDbs.GetProfileHarvestUploads(profileId);
            var profileInfo = _profileDbs.GetPrivateProfile(profileId);
            var privateProfile = profileInfo with
            {
                HarvestUploads = profileHarvests.ToList(),
            };

            _logger.LogDebug("GetPrivProfile success. profileId={ProfileId}, harvestUploadsCount={HarvestUploadsCount}", profileId, privateProfile.HarvestUploads?.Count ?? 0);

            return privateProfile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetPrivProfile failed. profileId={ProfileId}", profileId);
            throw;
        }
    }
    
    public void UpdateProfile(PrivateProfileDto dto) //update
    {
        try
        {
            _logger.LogInformation("UpdateProfile called. profileId={ProfileId}", dto.ProfileId);
            
            _profileDbs.EditProfile(dto);
            
            _logger.LogDebug("UpdateProfile success. profileId={ProfileId}", dto.ProfileId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateProfile failed. profileId={ProfileId}", dto.ProfileId);
            throw new InvalidOperationException("Profil konnte nicht mit neuen Informationen aktualisert werden.", ex);
        }
    }

    //Int als Rückgabe, weil beim Registrierungsprozess für Präferenzen und HarvestUpload die ProfileId benötigen
    public int RegisterProfile(PrivateProfileDto newProfile, CredentialProfileDto credentials)
    {
        try
        {
            _logger.LogInformation("RegisterProfile called. userName={UserName}, email={Email}", newProfile.UserName, newProfile.EMail);
            
            var profileId = _profileDbs.SetNewProfile(newProfile, credentials);
            
            _logger.LogInformation("RegisterProfile success. profileId={ProfileId}, userName={UserName}", profileId, newProfile.UserName);
            
            return profileId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RegisterProfile failed. userName={UserName}, email={Email}", newProfile.UserName, newProfile.EMail);
            throw new InvalidOperationException("Registrierung konnte nicht durchgeführt werden.", ex);
        }
    }
    
    public PrivateProfileDto LoginProfile(CredentialProfileDto credentials)
    {
        _logger.LogInformation("LoginProfile called. email={Email}", credentials.EMail);
        
        int? profileId;

        //Überprüf, ob eingegebene Credentials korrekt waren
        try
        {
            profileId = _profileDbs.CheckPassword(credentials.EMail, credentials.PasswordHash);
            _logger.LogDebug("LoginProfile password check completed. email={Email}, profileId={ProfileId}", credentials.EMail, profileId);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Login konnte technisch nicht durchgeführt werden.", ex);
        }

        //Falls nicht -> Exception werfen
        if (!profileId.HasValue)
        {
            _logger.LogWarning("LoginProfile failed: invalid credentials. email={Email}", credentials.EMail);
            throw new UnauthorizedAccessException("Ungültige Anmeldedaten");
        }

        //Benötigte Profilinformationen von der Datenbank holen
        var profileHarvests = _harvestDbs.GetProfileHarvestUploads(profileId.Value);
        var profileInfo = _profileDbs.GetPrivateProfile(profileId.Value);

        //ProfileInfo um HarvestUploads ergänzen
        _logger.LogInformation("LoginProfile success. profileId={ProfileId}, email={Email}", profileId.Value, credentials.EMail);
        return profileInfo with
        {
            HarvestUploads = profileHarvests.ToList(),
        };
    }

    public void UpdateCredentials(CredentialProfileDto credentials)
    {
        try
        {
            _logger.LogInformation("UpdateCredentials called. email={Email}", credentials.EMail);
            
            _profileDbs.EditPassword(credentials);
            
            _logger.LogDebug("UpdateCredentials success. email={Email}", credentials.EMail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateCredentials failed. email={Email}", credentials.EMail);
            throw new InvalidOperationException("Anmeldedaten konnten nicht aktualisiert werden.", ex);
        }
    }

    public void SetPreference(List<PreferenceDto> preferences)
    {
        try
        {
            _logger.LogInformation("SetPreference called. preferencesCount={PreferencesCount}", preferences?.Count ?? 0);
            
            _profileDbs.SetProfilePreference(preferences);
            
            _logger.LogDebug("SetPreference success. preferencesCount={PreferencesCount}", preferences?.Count ?? 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SetPreference failed. preferencesCount={PreferencesCount}", preferences?.Count ?? 0);
            throw new InvalidOperationException("Präferenzen konnten nicht gespeichert werden.", ex);
        }
    }
}