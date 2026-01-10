using Contracts.Data_Transfer_Objects;
using DataManagement.Entities;
using DataManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataManagement;

public class ProfileDbs : IProfileDbs
{
    private readonly EliteGaertnerDbContext _dbContext;
    private readonly ILogger<ProfileDbs> _logger;
    
    public ProfileDbs(EliteGaertnerDbContext dbContext, ILogger<ProfileDbs> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }   
    
    public int? CheckPassword(string eMail, string klartextPassword)
    {
        _logger.LogDebug("CheckPassword called. EmailProvided={EmailProvided}", !string.IsNullOrWhiteSpace(eMail));
        
        if (string.IsNullOrWhiteSpace(eMail) || string.IsNullOrWhiteSpace(klartextPassword))
            return null;
        
        var normalizedEmail = eMail.Trim().ToLowerInvariant();
        var profile = _dbContext.Profiles
            .AsNoTracking()
            .SingleOrDefault(p => 
                p.Email == normalizedEmail);
        
        if (profile == null)
        {
            _logger.LogInformation("CheckPassword: profile not found for email={Email}", normalizedEmail);
            return null;
        }

        bool isValid = BCrypt.Net.BCrypt.Verify(klartextPassword, profile.Passwordhash);
        
        _logger.LogInformation("CheckPassword completed. Email={Email}, IsValid={IsValid}", normalizedEmail, isValid);
        
        return isValid ? profile.Profileid : null;
    }
    

    public bool CheckProfileNameExists(string username)
    {
        _logger.LogDebug("CheckProfileNameExists called. UsernameProvided={UsernameProvided}", !string.IsNullOrWhiteSpace(username));
        
        // Falls username null oder leer -> true keine Vergabe
        if (string.IsNullOrWhiteSpace(username))
            return false;
    
        // Normalisiere username (wie in SetNewProfile)
        var normalizedUsername = username.Trim().ToLowerInvariant();
        
        var exists = _dbContext.Profiles
            .AsNoTracking()
            .Any(p => p.Username == normalizedUsername);

        _logger.LogInformation("CheckProfileNameExists completed. Username={Username}, Exists={Exists}", normalizedUsername, exists);

        return exists;
    }
    
    public int SetNewProfile(PrivateProfileDto privateProfile, CredentialProfileDto credentials)
    {
        _logger.LogInformation("SetNewProfile called.");
        
        //Überprüfungen
        if (privateProfile is null)
            throw new ArgumentNullException(nameof(privateProfile));
        if (credentials is null)
            throw new ArgumentNullException(nameof(credentials));
        
        //UserName und Email normalisieren. Uppercase -> Lowercase
        var userName = privateProfile.UserName?.Trim().ToLowerInvariant();
        var eMail = credentials.EMail?.Trim().ToLowerInvariant();
        _logger.LogDebug("SetNewProfile normalized inputs. Username={Username}, Email={Email}", userName, eMail);
        
        //Pflichtfelder vorhanden?
        if (string.IsNullOrWhiteSpace(userName) ||
            string.IsNullOrWhiteSpace(eMail) ||
            string.IsNullOrWhiteSpace(credentials.PasswordHash))
            //Falls nicht -> return leeres PrivateProfileDto
            throw new ArgumentException(
                "Username, E-Mail und Passwort sind Pflichtfelder."
            );
        
        //Prüfe Mail und Username auf Duplikate
        var userNameExists = _dbContext.Profiles
            .AsNoTracking()
            .Any(p => p.Username == userName);
        var eMailExists = _dbContext.Profiles
            .AsNoTracking()
            .Any(p => p.Email == eMail);
        _logger.LogDebug("SetNewProfile duplicate checks. UsernameExists={UsernameExists}, EmailExists={EmailExists}", userNameExists, eMailExists);
        
        //Exceptions bei Duplikate
        if (userNameExists)
            throw new InvalidOperationException("Der Benutzername ist bereits vergeben.");
        if (eMailExists)
            throw new InvalidOperationException("Die Email ist bereits vergeben.");
        
        //Passwort hashen (BCrypt)
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(credentials.PasswordHash);
        
        //Dto auf Entity mappen
        var profileEntity = new Profile()
        {
            Profilepictureurl = privateProfile.ProfilepictureUrl,
            Username = userName,
            Firstname = privateProfile.FirstName,
            Lastname = privateProfile.LastName,
            //Die Email und Passwort kommen aus dem CredentialDto
            Email = eMail,
            Passwordhash = hashedPassword,
            Phonenumber = privateProfile.Phonenumber,
            Profiletext = privateProfile.Profiletext,
            Sharemail = privateProfile.ShareMail,
            Sharephonenumber = privateProfile.SharePhoneNumber,
            Usercreated = DateTime.UtcNow
        };

        //Entity wird in Datenbank gespeichert
        _dbContext.Profiles.Add(profileEntity);
        _dbContext.SaveChanges();
        _logger.LogInformation("SetNewProfile succeeded. NewProfileId={ProfileId}, Username={Username}", profileEntity.Profileid, userName);

        //ProfileId zurückgeben
        return profileEntity.Profileid;
    }

    public void EditProfile(PrivateProfileDto privateProfile)
    {
        _logger.LogInformation("EditProfile called. ProfileId={ProfileId}", privateProfile?.ProfileId);
        if (privateProfile == null || privateProfile.ProfileId <= 0)
            throw new ArgumentNullException(nameof(privateProfile), "Dto darf nicht null sein, oder ID negativ");
   
        // Profil aus Datenbank laden 
        var profile = _dbContext.Profiles
            .SingleOrDefault(p => p.Profileid == privateProfile.ProfileId);

        if (profile == null)
        {
            _logger.LogWarning("EditProfile: profile not found. ProfileId={ProfileId}", privateProfile.ProfileId);
            throw new ArgumentException($"Profil mit ID {privateProfile.ProfileId} nicht gefunden.", nameof(privateProfile.ProfileId));
        }

        // DTO-Werte auf Entity mappen (alle editierbaren Felder)
        profile.Profilepictureurl = privateProfile.ProfilepictureUrl;
        profile.Email = privateProfile.EMail;
        profile.Phonenumber = privateProfile.Phonenumber;
        profile.Profiletext = privateProfile.Profiletext;
        profile.Sharemail = privateProfile.ShareMail;
        profile.Sharephonenumber = privateProfile.SharePhoneNumber;
    
        //Email und PhoneNumber nur bei Nicht-Null überschreiben
        if (!string.IsNullOrWhiteSpace(privateProfile.EMail))
            profile.Email = privateProfile.EMail.Trim().ToLowerInvariant();
        if (!string.IsNullOrWhiteSpace(privateProfile.Phonenumber))
            profile.Phonenumber = privateProfile.Phonenumber?.Trim();

        //Änderungen persistieren
        _dbContext.SaveChanges();
        
        _logger.LogInformation("EditProfile succeeded. ProfileId={ProfileId}", privateProfile.ProfileId);
    }
    
    public Profile? GetProfile(int profileId)
    {
        _logger.LogDebug("GetProfile called. ProfileId={ProfileId}", profileId);
        
        //Falls die profileId <= 0 ist, return ein leeres ProfileDto
        if (profileId <= 0)
            return null;
        
        //Fragt die Datenbank nach dem Profil ab und speichert es in die Entität
        var p = _dbContext.Profiles
            //Da es sich hier um eine Read Only Abfrage handelt, muss sich EF kein Objekt merken
            .AsNoTracking()
            //Gib mir maximal ein Profil zurück, dass mit der Id übereinstimmt
            .SingleOrDefault(x => x.Profileid == profileId);
        
        _logger.LogDebug("GetProfile completed. ProfileId={ProfileId}, Found={Found}", profileId, p is not null);
        
        return p;
    }
    
    public PublicProfileDto GetPublicProfile(int profileId)
    {
        _logger.LogDebug("GetPublicProfile called. ProfileId={ProfileId}", profileId);
        
        //Entität Profil wird zurückgegeben
        var p = GetProfile(profileId);

        //Falls kein Profil gefunden wird -> Gib leeres PublicProfileDto zurück
        if (p is null)
        {
            _logger.LogInformation("GetPublicProfile: profile not found. ProfileId={ProfileId}", profileId);
            return new PublicProfileDto();
        }
        
        //Erstelle aus dem Ergebnis der Query ein PublicProfileDto
        var result = new PublicProfileDto()
        {
            ProfileId = p.Profileid,
            ProfilepictureUrl = p.Profilepictureurl,
            UserName = p.Username,
            Profiletext = p.Profiletext,
            UserCreated = p.Usercreated,
        };
        
        _logger.LogDebug("GetPublicProfile completed. ProfileId={ProfileId}, Username={Username}", profileId, result.UserName);
        
        return result;
    }

    public PrivateProfileDto GetPrivateProfile(int profileId)
    {
        _logger.LogDebug("GetPrivateProfile called. ProfileId={ProfileId}", profileId);
        
        //Entität Profil wird zurückgegeben
        var p = GetProfile(profileId);

        //Falls kein Profil gefunden wird -> Gib leeres PrivateProfileDto zurück
        if (p is null)
        {
            _logger.LogInformation("GetPrivateProfile: profile not found. ProfileId={ProfileId}", profileId);
            return new PrivateProfileDto();
        }
        
        //Erstelle aus dem Ergebnis der Query ein PrivateProfileDto
        var result = new PrivateProfileDto
        {
            ProfileId = p.Profileid,
            UserName = p.Username,
            ProfilepictureUrl = p.Profilepictureurl,
            FirstName = p.Firstname,
            LastName = p.Lastname,
            EMail = p.Email,
            Phonenumber = p.Phonenumber,
            Profiletext = p.Profiletext,
            ShareMail = p.Sharemail,
            SharePhoneNumber = p.Sharephonenumber,
            //Harvestuploads werden über eine andere Klasse geholt!
            //Hol dir die UserPreference des Profils
            PreferenceDtos = GetProfilePreference(profileId).ToList()
        };
        _logger.LogDebug("GetPrivateProfile completed. ProfileId={ProfileId}, Username={Username}, PreferencesCount={PreferencesCount}", profileId, result.UserName, result.PreferenceDtos?.Count ?? 0);
        
        return result;
    }
    
    public IEnumerable<PreferenceDto> GetProfilePreference(int profileId)
    {
        _logger.LogDebug("GetProfilePreference called. ProfileId={ProfileId}", profileId);
        
        //Falls die profileId <= 0 ist, return ein leeres PreferenceDto Enumerable 
        if (profileId <= 0)
            return Enumerable.Empty<PreferenceDto>();

        var result = _dbContext.Profilepreferences
            //Da es sich hier um eine Read Only Abfrage handelt, muss sich EF kein Objekt merken
            .AsNoTracking()
            //Gib mir alle ProfilePreferences des Profils zurück
            .Where(p => p.Profileid == profileId)
            //Erstelle aus dem Ergebnis der Query die PreferenceDtos
            .Select(p => new PreferenceDto()
            {
                Profileid = p.Profileid,
                TagId = p.Tagid,
                DateUpdated = p.Dateupdated
            });
        _logger.LogDebug("GetProfilePreference query built. ProfileId={ProfileId}", profileId);

        return result;
    }

    public void EditPassword(CredentialProfileDto credentials)
    {
        _logger.LogInformation("EditPassword called. EmailProvided={EmailProvided}", !string.IsNullOrWhiteSpace(credentials?.EMail));
        
        //Überprüfungen
        if (credentials is null)
            throw new ArgumentNullException(nameof(credentials));
        //Trim die Mail -> lowercases
        var email = credentials.EMail?.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("E-Mail ist erforderlich.", nameof(credentials.EMail));
        if (string.IsNullOrWhiteSpace(credentials.PasswordHash))
            throw new ArgumentException("Passwort/Hash ist erforderlich.", nameof(credentials.PasswordHash));

        //Holt das gesuchte Profil aus der Datenbank
        var profile = _dbContext.Profiles.SingleOrDefault(p => p.Email == email);
        if (profile is null)
        {
            _logger.LogWarning("EditPassword: profile not found. Email={Email}", email);
            throw new ArgumentException("Profil mit dieser E-Mail nicht gefunden.", nameof(credentials.EMail));
        }

        //Passwort hashen (BCrypt)
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(credentials.PasswordHash);
        profile.Passwordhash = hashedPassword;

        _dbContext.SaveChanges();
        
        _logger.LogInformation("EditPassword succeeded. Email={Email}", email);
    }

    public void SetProfilePreference(List<PreferenceDto> preferences) // Wichtiger Hinweis: Methode überschreibt nur für
                                                                   // eine ProfileID (die erste in der Liste), damit
                                                                   // nicht aus Versehen andere Nutzer editiert werden
    {
        _logger.LogInformation("SetProfilePreference called. PreferencesCount={PreferencesCount}", preferences?.Count ?? 0);
        
        // Input-Validierung (Write-Operation -> Exceptions)
        if (preferences is null)
            throw new ArgumentNullException(nameof(preferences));

        if (preferences.Count == 0)
            throw new ArgumentException("Die Preference-Liste darf nicht leer sein.", nameof(preferences));

        // ProfileId muss vorhanden und bei allen gleich sein
        var profileId = preferences[0].Profileid;
        _logger.LogDebug("SetProfilePreference target profile. ProfileId={ProfileId}", profileId);
        
        if (profileId <= 0)
            throw new ArgumentException("ProfileId muss größer als 0 sein.", nameof(preferences));

        if (preferences.Any(p => p.Profileid != profileId))
            throw new ArgumentException("Alle PreferenceDtos müssen die gleiche ProfileId haben.", nameof(preferences));

        // Optional: TagId validieren
        if (preferences.Any(p => p.TagId <= 0))
            throw new ArgumentException("TagId muss größer als 0 sein.", nameof(preferences));

        // 1) Bestehende Preferences dieses Profils löschen
        var existingPreferences = _dbContext.Profilepreferences
            .Where(pp => pp.Profileid == profileId);
        var existingCount = existingPreferences.Count();
        _logger.LogDebug("SetProfilePreference removing existing preferences. ProfileId={ProfileId}, ExistingCount={ExistingCount}", profileId, existingCount);

        _dbContext.Profilepreferences.RemoveRange(existingPreferences);

        // 2) Neue Preferences hinzufügen
        var newPreferences = preferences.Select(p => new Profilepreference
        {
            Tagid = p.TagId,
            Profileid = p.Profileid,
            Dateupdated = p.DateUpdated == default
                ? DateTime.UtcNow
                : p.DateUpdated
        }).ToList();
        _logger.LogDebug("SetProfilePreference adding new preferences. ProfileId={ProfileId}, NewCount={NewCount}", profileId, newPreferences.Count);

        _dbContext.Profilepreferences.AddRange(newPreferences);

        _dbContext.SaveChanges();
        
        _logger.LogInformation("SetProfilePreference succeeded. ProfileId={ProfileId}, NewCount={NewCount}", profileId, newPreferences.Count);
    }
}