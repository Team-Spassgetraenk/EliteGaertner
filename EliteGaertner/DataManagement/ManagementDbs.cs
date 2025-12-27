using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;
using DataManagement.Entities;
using DataManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataManagement;

//TODO Reads Prüfungen -> Null zurückgeben, Write Prüfungen -> Exceptions implementieren
//TODO Prüfungen vielleicht in einer Methode zusammenführen
public class ManagementDbs : IHarvestDbs, IMatchesDbs, IPreferenceDbs, IProfileDbs
{
    private readonly EliteGaertnerDbContext _dbContext;

    public ManagementDbs(EliteGaertnerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<HarvestUploadDto> GetProfileHarvestUploads(int profileId)
    {
        //Falls die profileId <= 0 ist, return leere HarvestUploadDto Enumerable 
        if (profileId <= 0)
            return Enumerable.Empty<HarvestUploadDto>();

        var result = _dbContext.Harvestuploads
            .AsNoTracking()
            .Where(h => h.Profileid == profileId)
            .Select(h => new HarvestUploadDto()
            {
                UploadId = h.Uploadid,
                ImageUrl = h.Imageurl,
                Description = h.Description,
                WeightGram = h.Weightgramm,
                WidthCm = h.Widthcm,
                LengthCm = h.Lengthcm,
                UploadDate = h.Uploaddate,
                ProfileId = h.Profileid
            });

        return result;
    }

    
    public bool CreateUploadDbs(HarvestUploadDto uploadDto)
    {
        var entity = new Harvestupload
        {

            Imageurl = uploadDto.ImageUrl,
            Description = uploadDto.Description,
            Weightgramm = uploadDto.WeightGram,
            Widthcm = uploadDto.WidthCm,
            Lengthcm = uploadDto.LengthCm,
            Uploaddate = uploadDto.UploadDate,
            Profileid = uploadDto.ProfileId
        };

        _dbContext.Harvestuploads.Add(entity);
        var rows = _dbContext.SaveChanges(); //checkt ob db überhaupt schreibt
        return rows > 0;
        
    }

    //TODO LÖSCHUNG funktioniert noch nicht, da ich noch nicht die Abhängigkeiten mitlösche
    public bool DeleteHarvestUpload(int uploadId)
    {
        if (uploadId <= 0)
            throw new ArgumentOutOfRangeException(nameof (uploadId), "UploadId muss größer als 0 sein.");
        
        //Existiert überhaupt Upload?
        var uploadIdExists = _dbContext.Harvestuploads
            .AsNoTracking()
            .Any(h => h.Uploadid == uploadId);

        if (!uploadIdExists)
            throw new ArgumentException("Kein HarvestUpload mit dieser UploadId auffindbar!", nameof(uploadId));

        //Finde das gesuchte Bild
        var deleteUpload = _dbContext.Harvestuploads
            .Single(h => h.Uploadid == uploadId);

        //Lösche es
        _dbContext.Remove(deleteUpload);
        _dbContext.SaveChanges();
        return true;
    }
    
    public void SetReportHarvestUpload(int uploadId, ReportReasons reason)
    {
        if (uploadId <= 0)
            throw new ArgumentOutOfRangeException(nameof (uploadId), "UploadId muss größer als 0 sein.");
        
        //Existiert überhaupt Upload?
        var uploadIdExists = _dbContext.Harvestuploads
            .AsNoTracking()
            .Any(h => h.Uploadid == uploadId);

        if (!uploadIdExists)
            throw new ArgumentException("Kein HarvestUpload mit dieser UploadId auffindbar!", nameof(uploadId));

        var report = new Report
        {
            Reason = reason.ToString(),
            Reportdate = DateTime.UtcNow,
            Uploadid = uploadId,
        };

        _dbContext.Reports.Add(report);
        _dbContext.SaveChanges();
    }
    
    public int GetReportCount(int uploadId)
    {
        //Falls ungültige ID -> 0 zurück
        if (uploadId <= 0)
            return 0;
        
        //Existiert überhaupt Upload?
        var uploadIdExists = _dbContext.Harvestuploads
            .AsNoTracking()
            .Any(h => h.Uploadid == uploadId);

        //Falls ungültige ID -> 0 zurück
        if (!uploadIdExists)
            return 0;

        //Gib die Anzahl der Reports zurück
        return _dbContext.Reports
            .AsNoTracking()
            .Count(r => r.Uploadid == uploadId);
    }
    
    //TODO Ich muss den Output randomisieren, sonst findet der irgendwann nicht mehr die richtigen Bilder
    public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(int profileId, List<int> tagIds, int preloadCount)
    {
        //Überprüfung ob ProfilId, TagIds vorhanden sind und PreloadCount > 0 ist
        //Falls nicht -> leere Liste zurückgeben
        if (profileId <= 0 || tagIds is null || tagIds.Count == 0 || preloadCount <= 0)
            return Enumerable.Empty<HarvestUploadDto>();
        
        //Hier bin ich in einen bekannten EF Core Query Translation Bug gelaufen.
        //Anscheinend mag er die Kombi aus GroupBy, Select(g => g.OrderByDescending(...).First()), Select(new Dto) nicht.
        //-> KeyNotFoundException: EmptyProjectionMember 
        //Deswegen musste ich die Query "aufsplitten"
        var latestPerProfile = _dbContext.Harvestuploads
            //Da es sich hier um eine Read Only Abfrage handelt, muss sich EF kein Objekt merken
            .AsNoTracking()
            //Filter alle HarvestUploads des Content Receivers heraus
            .Where(h => h.Profileid != profileId)
            //Überprüfe alle Tags des Harvest Uploads und prüfe, ob es mind ein Tag gibt, dass mit den Interessen
            //des Content Receivers übereinstimmen
            .Where(h => h.Tags.Any(t => tagIds.Contains(t.Tagid)))
            //Gruppiere die HarvestUploads die dem gleichen User gehören
            //Finde heraus welches dieser Uploads am aktuellsten ist und speicher dies in Objekten ab
            .GroupBy(h => h.Profileid)
            .Select(g => new
            {
                ProfileId = g.Key,
                MaxUploadDate = g.Max(x => x.Uploaddate)
            });

        //Hauptquery: Join auf (ProfileId, UploadDate) => genau diese neuesten Uploads
        //Dann sortieren + Take(preloadCount) und erst ganz am Ende materialisieren.
        var result = (
                from h in _dbContext.Harvestuploads.AsNoTracking()
                //Wir joinen jetzt das Ergebnis von latestPerProfile mit der HarvestUpload Tabelle
                //Somit bleiben nur noch die aktuellsten Uploads der User die mit unseren Interessen übereinstimmen
                join l in latestPerProfile
                    on new { ProfileId = h.Profileid, UploadDate = h.Uploaddate }
                    equals new { ProfileId = l.ProfileId, UploadDate = l.MaxUploadDate }
                //Redundante Absicherung um eigene Uploads zu vermeiden
                where h.Profileid != profileId
                //Überprüft nochmal ob der Tag im Upload vorhanden ist
                where h.Tags.Any(t => tagIds.Contains(t.Tagid))
                //Sortiert nach den aktuellsten Uploads
                orderby h.Uploaddate descending
                //Erstellt aus den Ergebnissen die benötigten Dtos
                select new HarvestUploadDto
                {
                    UploadId = h.Uploadid,
                    ImageUrl = h.Imageurl,
                    Description = h.Description,
                    WeightGram = h.Weightgramm,
                    WidthCm = h.Widthcm,
                    LengthCm = h.Lengthcm,
                    UploadDate = h.Uploaddate,
                    ProfileId = h.Profileid
                })
            //PreloadCount gibt die Anzahl der benötigten DTOs an
            .Take(preloadCount)
            .ToList();

        return result;
    }

    public void SetHarvestUpload(HarvestUploadDto harvestUpload)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<PublicProfileDto> GetActiveMatches(int profileIdReceiver)
    {
        //Überprüfe, ob ProfileId <= 0 ist. Falls ja -> Gib leere Liste zurück 
        if (profileIdReceiver <= 0)
            return Enumerable.Empty<PublicProfileDto>();
        
        //Finde alle Ratings, in denen sich der Receiver und der Creator gegenseitig
        //positiv bewertet haben -> Gib mir am Ende die CreatorIds zurück 
        var matches =
            //Nimm die Tabelle Ratings und mach ein Selfjoin und überprüfe, 
            //ob Receiver und Creator sich gegenseitig bewertet haben
            (from r1 in _dbContext.Ratings.AsNoTracking()
                join r2 in _dbContext.Ratings.AsNoTracking()
                    on new { A = r1.Contentreceiverid, B = r1.Contentcreatorid }
                    equals new { A = r2.Contentcreatorid, B = r2.Contentreceiverid }
                //Zeig alle Ratings vom Receiver 
                where r1.Contentreceiverid == profileIdReceiver
                      //Überprüfe, ob sich beide gegenseitig positiv bewertet haben
                      && r1.Profilerating
                      && r2.Profilerating
                //Speicher die Id vom Creator ab      
                select r1.Contentcreatorid)
            //Eigentlich nicht möglich, da ProfileIds PK sind, aber vorsichtshalber 
            //können ProfileIds nicht mehrmals vorkommen
            .Distinct();

        //Gleich alle profileIds mit der Profiles-Tabelle ab und erstelle aus ihnen die PublicProfileDto
        var result =
            //Nimm jede ProfileId aus Matches
            (from pid in matches
                //Macht einen Join mit den Inhalten der Profiles-Tabelle die mit profileIds aus matches übereinstimmen 
                join p in _dbContext.Profiles.AsNoTracking()
                    on pid equals p.Profileid
                //Aus den Ergebnissen werden die passenden PublicProfileDtos erstellt     
                select new PublicProfileDto
                {
                    //Wir benötigen für die Matchübersicht nur ProfileId, Username
                    //und möglicherweise Email und Phonenumber 
                    ProfileId = p.Profileid,
                    UserName = p.Username,
                    //Prüft ob der Creator seine Mail und/oder Telefonnummer freigegeben hat
                    EMail = p.Sharemail ? p.Email : null,
                    Phonenumber = p.Sharephonenumber ? p.Phonenumber : null,
                })
            .ToList();

        return result;
    }

    public bool ProfileAlreadyRated(int profileIdReceiver, int profileIdCreator)
    {
        return _dbContext.Ratings                    
            .Any(r =>                               
                r.Contentreceiverid == profileIdReceiver &&
                r.Contentcreatorid == profileIdCreator); 
    }
    
    public void SaveMatchInfo(RateDto matchDto)
    {
        //Prüfung, ob matchDto null ist -> ArgumentNullException
        if (matchDto is null)
            throw new ArgumentNullException(nameof(matchDto), "matchDto darf nicht null sein und kann nicht gespeichert werden.");
        //Prüfung der Inhalte der MatchDto -> ArgumentException
        if (matchDto.ContentCreator <= 0 || matchDto.ContentReceiver <= 0 ||
            matchDto.ContentCreator == matchDto.ContentReceiver)
            throw new ArgumentException("matchDto hat inhaltliche Fehler und kann nicht gespeichert werden.",
                nameof(matchDto));

        //Prüft, ob Eintrag bereits vorhanden ist
        var alreadyRated = _dbContext.Ratings
            .SingleOrDefault(r => 
                r.Contentreceiverid == matchDto.ContentReceiver &&
                r.Contentcreatorid == matchDto.ContentCreator);

        //Falls nicht -> Füge Rating zu Datenbank
        if (alreadyRated is null)
        {
            var rating = new Rating
            {
                Contentreceiverid = matchDto.ContentReceiver,
                Contentcreatorid = matchDto.ContentCreator,
                Profilerating = matchDto.ContentReceiverValue,
                Ratingdate = matchDto.ContentReceiverRatingDate,
            };
            _dbContext.Ratings.Add(rating);
        }
        //Falls schon -> Update die Zeile in der Datenbank
        //Das kann eigentlich nicht passieren, weil nur User zur Bewertung vorgeschlagen werden, die noch nie 
        //bewertet worden sind. Aber falls man in Zukunft das doch erlaubt -> Update in der Datenbank
        else
        {
            alreadyRated.Profilerating = matchDto.ContentReceiverValue;
            alreadyRated.Ratingdate = matchDto.ContentReceiverRatingDate;
        }
        
        _dbContext.SaveChanges();
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

    public void SetUserPreference(int userId, PreferenceDto newUserPreference)
    {
        throw new NotImplementedException();
    }
    
    public PrivateProfileDto SetNewProfile(PrivateProfileDto privateProfile, CredentialProfileDto credentials)
    {
        
        //Es ist nicht möglich, Stand jetzt, eine PrivateProfileDto/CredentialProfileDto
        //zu übergeben die = null ist. Trotzdem defensive Programmierung.
        if (privateProfile is null || credentials is null)
            return new PrivateProfileDto();
        
        //UserName und Email normalisieren. Uppercase -> Lowercase
        var userName = privateProfile.UserName?.Trim().ToLowerInvariant();
        var eMail = credentials.EMail?.Trim().ToLowerInvariant();
        
        //Pflichtfelder vorhanden?
        if (string.IsNullOrWhiteSpace(userName) ||
            string.IsNullOrWhiteSpace(eMail) ||
            string.IsNullOrWhiteSpace(credentials.PasswordHash))
            //Falls nicht -> return leeres PrivateProfileDto
            return new PrivateProfileDto();
        
        //Prüfe Mail und Username auf Duplikate
        var userNameExists = _dbContext.Profiles
            .AsNoTracking()
            .Any(p => p.Username == userName);
        var eMailExists = _dbContext.Profiles
            .AsNoTracking()
            .Any(p => p.Email == eMail);
        //Falls UserName oder Passwort schon existiert -> leeres ProfileDto wird zurückgegeben
        //Nicht in einem Statement zusammengelegt, um für die Zukunft festellen zu können
        //welcher Wert bereits vorhanden ist
        if (userNameExists)
            return new PrivateProfileDto();
        if (eMailExists)
            return new PrivateProfileDto();
        
        //Dto auf Entity mappen
        var profileEntity = new Profile()
        {
            Profilepictureurl = privateProfile.ProfilepictureUrl,
            Username = userName,
            Firstname = privateProfile.FirstName,
            Lastname = privateProfile.LastName,
            //Die Email und Passwort kommen aus dem CredentialDto
            Email = eMail,
            Passwordhash = credentials.PasswordHash,
            Phonenumber = privateProfile.Phonenumber,
            Profiletext = privateProfile.Profiletext,
            Sharemail = privateProfile.ShareMail,
            Sharephonenumber = privateProfile.SharePhoneNumber,
            Usercreated = DateTime.UtcNow
        };

        //Entity wird in Datenbank gespeichert
        _dbContext.Profiles.Add(profileEntity);
        _dbContext.SaveChanges();
        
        //Dto wird erstellt und zurückgegeben
        var result = new PrivateProfileDto()
        {
            ProfileId = profileEntity.Profileid,
            ProfilepictureUrl = profileEntity.Profilepictureurl,
            UserName = profileEntity.Username,
            FirstName = profileEntity.Firstname,
            LastName = profileEntity.Lastname,
            EMail = profileEntity.Email,
            Phonenumber = profileEntity.Phonenumber,
            Profiletext = profileEntity.Profiletext,
            ShareMail = profileEntity.Sharemail,
            SharePhoneNumber = profileEntity.Sharephonenumber,
            UserCreated = profileEntity.Usercreated,
        };

        return result;
    }

    //TODO Nicolas
    public PrivateProfileDto EditProfile(PrivateProfileDto privateProfile)
    {
        throw new NotImplementedException();
    }

    public int? CheckPassword(string eMail, string passwordHash)
    {
        throw new NotImplementedException();
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
            //Hol dir die HarvestUploads des Profils
            HarvestUploads = GetProfileHarvestUploads(profileId).ToList(),
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
            FirstName = p.Firstname,
            LastName = p.Lastname,
            EMail = p.Email,
            Phonenumber = p.Phonenumber,
            Profiletext = p.Profiletext,
            ShareMail = p.Sharemail,
            SharePhoneNumber = p.Sharephonenumber,
            //Hol dir die HarvestUploads des Profils
            HarvestUploads = GetProfileHarvestUploads(profileId).ToList(),
            //Hol dir die UserPreference des Profils
            PreferenceDtos = GetUserPreference(profileId).ToList()
        };

        return result;
    }
}
