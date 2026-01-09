using Contracts.Data_Transfer_Objects;
using DataManagement.Entities;
using DataManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataManagement;

public class ProfileDbs : IProfileDbs
{
    private readonly EliteGaertnerDbContext _dbContext;
    
    public ProfileDbs(EliteGaertnerDbContext dbContext)
    {
        _dbContext = dbContext;
    }   
    
    public int? CheckPassword(string eMail, string klartextPassword)
    {
        if (string.IsNullOrWhiteSpace(eMail) || string.IsNullOrWhiteSpace(klartextPassword))
            return null;
        
        var normalizedEmail = eMail.Trim().ToLowerInvariant();
        var profile = _dbContext.Profiles
            .AsNoTracking()
            .SingleOrDefault(p => 
                p.Email == normalizedEmail);
        
        if (profile == null)
            return null;

        bool isValid = BCrypt.Net.BCrypt.Verify(klartextPassword, profile.Passwordhash);
    
        return isValid ? profile.Profileid : null;
    }
    

    public bool CheckUsernameExists(string username)
    {
        // Falls username null oder leer -> true keine Vergabe
        if (string.IsNullOrWhiteSpace(username))
            return false;
    
        // Normalisiere username (wie in SetNewProfile)
        var normalizedUsername = username.Trim().ToLowerInvariant();
        
        return _dbContext.Profiles
            .AsNoTracking()
            .Any(p => p.Username == normalizedUsername);
    }
    
    public int SetNewProfile(PrivateProfileDto privateProfile, CredentialProfileDto credentials)
    {
        //Überprüfungen
        if (privateProfile is null)
            throw new ArgumentNullException(nameof(privateProfile));
        if (credentials is null)
            throw new ArgumentNullException(nameof(credentials));
        
        //UserName und Email normalisieren. Uppercase -> Lowercase
        var userName = privateProfile.UserName?.Trim().ToLowerInvariant();
        var eMail = credentials.EMail?.Trim().ToLowerInvariant();
        
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

        //ProfileId zurückgeben
        return profileEntity.Profileid;
    }

    public void EditProfile(PrivateProfileDto privateProfile)
    {
        if (privateProfile == null || privateProfile.ProfileId <= 0)
            throw new ArgumentNullException(nameof(privateProfile), "Dto darf nicht null sein, oder ID negativ");
   
        // Profil aus Datenbank laden 
        var profile = _dbContext.Profiles
            .SingleOrDefault(p => p.Profileid == privateProfile.ProfileId);

        if (profile == null)
            throw new ArgumentException($"Profil mit ID {privateProfile.ProfileId} nicht gefunden.", nameof(privateProfile.ProfileId));

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
    }
    
    public Profile? GetProfile(int profileId)
    {
        //Falls die profileId <= 0 ist, return ein leeres ProfileDto
        if (profileId <= 0)
            return null;
        
        //Fragt die Datenbank nach dem Profil ab und speichert es in die Entität
        var p = _dbContext.Profiles
            //Da es sich hier um eine Read Only Abfrage handelt, muss sich EF kein Objekt merken
            .AsNoTracking()
            //Gib mir maximal ein Profil zurück, dass mit der Id übereinstimmt
            .SingleOrDefault(x => x.Profileid == profileId);

        return p;
    }
    
    public PublicProfileDto GetPublicProfile(int profileId)
    {
        //Entität Profil wird zurückgegeben
        var p = GetProfile(profileId);

        //Falls kein Profil gefunden wird -> Gib leeres PublicProfileDto zurück
        if (p is null)
            return new PublicProfileDto();
        
        //Erstelle aus dem Ergebnis der Query ein PublicProfileDto
        var result = new PublicProfileDto()
        {
            ProfileId = p.Profileid,
            ProfilepictureUrl = p.Profilepictureurl,
            UserName = p.Username,
            Profiletext = p.Profiletext,
            UserCreated = p.Usercreated,
        };
        
        return result;
    }

    public PrivateProfileDto GetPrivateProfile(int profileId)
    {
        //Entität Profil wird zurückgegeben
        var p = GetProfile(profileId);

        //Falls kein Profil gefunden wird -> Gib leeres PrivateProfileDto zurück
        if (p is null)
            return new PrivateProfileDto();
        
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
            PreferenceDtos = GetUserPreference(profileId).ToList()
        };

        return result;
    }
    
    public IEnumerable<PreferenceDto> GetUserPreference(int profileId)
    {
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

        return result;
    }

    public void EditPassword(CredentialProfileDto credentials)
    {
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
        //Falls Profil nicht gefunden worden ist
        if (profile is null)
            throw new ArgumentException("Profil mit dieser E-Mail nicht gefunden.", nameof(credentials.EMail));

        //Passwort hashen (BCrypt)
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(credentials.PasswordHash);
        profile.Passwordhash = hashedPassword;

        _dbContext.SaveChanges();
    }

    public void SetUserPreference(List<PreferenceDto> preferences) // Wichtiger Hinweis: Methode überschreibt nur für
                                                                   // eine ProfileID (die erste in der Liste), damit
                                                                   // nicht aus Versehen andere Nutzer editiert werden
    {
        // Input-Validierung (Write-Operation -> Exceptions)
        if (preferences is null)
            throw new ArgumentNullException(nameof(preferences));

        if (preferences.Count == 0)
            throw new ArgumentException("Die Preference-Liste darf nicht leer sein.", nameof(preferences));

        // ProfileId muss vorhanden und bei allen gleich sein
        var profileId = preferences[0].Profileid;
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

        _dbContext.Profilepreferences.AddRange(newPreferences);

        _dbContext.SaveChanges();
    }
}