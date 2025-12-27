using System.Runtime.CompilerServices;
using Contracts.Data_Transfer_Objects;
using DataManagement.Entities;
using DataManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataManagement;

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

    public bool DeleteHarvestUpload(int uploadId)
    {
        throw new NotImplementedException();
    }

    public void SetReportHarvestUpload(int uploadId, Enum reason)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ReportDto> GetReportHarvestUpload(int uploadId)
    {
        throw new NotImplementedException();
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

    public bool CreateUploadDbs(HarvestUploadDto uploadDto)
    {
        var harvestUpload = new Harvestupload
        {
            Imageurl = uploadDto.ImageUrl,
            Description = uploadDto.Description ?? string.Empty,
            Weightgramm = uploadDto.WeightGram,
            Widthcm = uploadDto.WidthCm,
            Lengthcm = uploadDto.LengthCm,
            Uploaddate = uploadDto.UploadDate,
            Profileid = uploadDto.ProfileId
            //UploadID sollte von DB erstellt werden
        };

        try 
        {
            _dbContext.Harvestuploads.Add(harvestUpload);
            var affectedRows = _dbContext.SaveChanges();
            return affectedRows > 0; //TEst, wurde geschrieben
        }
        catch (Exception)
        {
            //TODO Loggen "Fehler beim Erstellen von HarvestUpload" 
            return false;
        }
    }

    public MatchDto GetMatchInfo(int profileIdReceiver, int profileIdCreator)
    {
        //Überprüfung, ob ProfileIds unterschiedlich sind und > 0 sind
        //Falls ja -> gib leere MatchDto zurück
        if (profileIdReceiver <= 0 || profileIdCreator <= 0 ||
            profileIdReceiver == profileIdCreator)
            return new MatchDto();

        var ratings = _dbContext.Ratings
            .AsNoTracking()
            .Where(r =>
                (r.Contentreceiverid == profileIdReceiver && r.Contentcreatorid == profileIdCreator) ||
                (r.Contentreceiverid == profileIdCreator && r.Contentcreatorid == profileIdReceiver))
            .ToList();
        
        //Gib mir, falls vorhanden, das Rating vom Receiver zurück
        var receiverRating = _dbContext.Ratings
            .SingleOrDefault(r =>
                r.Contentreceiverid == profileIdReceiver && r.Contentcreatorid == profileIdCreator);

        //Gib mir, falls vorhanden, das Rating vom Creator zurück
        var creatorRating = _dbContext.Ratings
            .SingleOrDefault(r =>
                r.Contentreceiverid == profileIdCreator && r.Contentcreatorid == profileIdReceiver);
        
        //MatchDto wird erstellt
        //Falls kein ReceiverRating oder CreatorRating vorhanden ist -> null
        return new MatchDto()
        {
            ContentReceiver = receiverRating?.Contentreceiverid,
            ContentReceiverValue = receiverRating?.Profilerating ?? false,

            ContentCreator = creatorRating?.Contentreceiverid,
            ContentCreatorValue = creatorRating?.Profilerating ?? false,

            ContentReceiverRatingDate = receiverRating?.Ratingdate,
            ContentCreatorRatingDate = creatorRating?.Ratingdate,
        };
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

    public void SaveMatchInfo(MatchDto matchDto)
    {
        throw new NotImplementedException();
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


    public PrivateProfileDto SetNewProfile(PrivateProfileDto privateProfile)
    {
        throw new NotImplementedException();
    }

    public PrivateProfileDto EditProfile(PrivateProfileDto privateProfile)
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
            UserName = p.Username,
            EMail = p.Email,
            Phonenumber = p.Phonenumber,
            Profiletext = p.Profiletext,
            ShareMail = p.Sharemail,
            SharePhoneNumber = p.Sharephonenumber,
            UserCreated = p.Usercreated,
            //Hol dir die HarvestUploads des Profils
            HarvestUploads = GetProfileHarvestUploads(profileId).ToList(),
            //Hol dir die UserPreference des Profils
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
            PasswordHash = p.Passwordhash,
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
