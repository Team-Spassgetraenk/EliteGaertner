using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;
using DataManagement.Interfaces;
using DataManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataManagement;

public class HarvestDbs : IHarvestDbs
{
    private readonly EliteGaertnerDbContext _dbContext;
    
    public HarvestDbs(EliteGaertnerDbContext dbContext)
    {
        _dbContext = dbContext;
    }   
    
    //TODO KOMMENTARE FEHLEN
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
                TagIds = h.Tags
                    .Select(t => t.Tagid)
                    .ToList(),
                ProfileId = h.Profileid
            });

        return result;
    }
        
    public void CreateUploadDbs(HarvestUploadDto uploadDto)
    {
        //Prüfungen
        if (uploadDto.ProfileId <= 0)
            throw new ArgumentException("Ungültige ProfileId");

        if (uploadDto.TagIds == null || uploadDto.TagIds.Count == 0)
            throw new ArgumentException("Mindestens ein Tag muss angegeben werden.");
        
        //Da TagIds doppelt in der HarvestUploadDto vorkommen könnten -> Duplikate entfernen
        var tagIds = uploadDto.TagIds
            .Where(id => id > 0)
            .Distinct()
            .ToList();
        
        //Tags aus der Datenbank holen
        var tags = _dbContext.Tags
            .Where(t => tagIds.Contains(t.Tagid))
            .ToList();
        //Überprüfe, ob mitgegebene Tags auch in der Datenbank vorhanden sind
        if (tags.Count != tagIds.Count)
            throw new InvalidOperationException("Mindestens ein Tag existiert nicht in der Datenbank.");
        
        var entity = new Harvestupload
        {
            Imageurl = uploadDto.ImageUrl,
            Description = uploadDto.Description,
            Weightgramm = uploadDto.WeightGram,
            Widthcm = uploadDto.WidthCm,
            Lengthcm = uploadDto.LengthCm,
            Uploaddate = DateTime.UtcNow,
            Tags = tags, 
            Profileid = uploadDto.ProfileId
        };

        _dbContext.Harvestuploads.Add(entity);
        _dbContext.SaveChanges();
    }

    public HarvestUploadDto GetUploadDb(int uploadId)
    {
        if (uploadId <= 0)
            return new HarvestUploadDto();

        var dto = _dbContext.Harvestuploads
            .AsNoTracking()
            .Where(h => h.Uploadid == uploadId)
            .Select(h => new HarvestUploadDto()
            {
                UploadId = h.Uploadid,
                ImageUrl = h.Imageurl,
                Description = h.Description,
                WeightGram = h.Weightgramm,
                WidthCm = h.Widthcm,
                LengthCm = h.Lengthcm,
                UploadDate = h.Uploaddate,
                TagIds = h.Tags
                    .Select(t => t.Tagid)
                    .ToList(), 
                ProfileId = h.Profileid
            })
            .SingleOrDefault();

        return dto ?? new HarvestUploadDto();
    }
    
    public void DeleteHarvestUpload(int uploadId)
    {
        //Prüfungen
        if (uploadId <= 0)
            throw new ArgumentOutOfRangeException(nameof (uploadId), "UploadId muss größer als 0 sein.");
        
        var deleteUpload = _dbContext.Harvestuploads
            .SingleOrDefault(h => h.Uploadid == uploadId);
        
        //Falls kein Upload gefunden wird
        if (deleteUpload is null)
            throw new ArgumentException(
                "Kein HarvestUpload mit dieser UploadId auffindbar!",
                nameof(uploadId));

        //Lösche es
        _dbContext.Remove(deleteUpload);
        _dbContext.SaveChanges();
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
    
    public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(
        int profileId, 
        List<int> tagIds, 
        HashSet<int> alreadyRatedProfiles, 
        int preloadCount)
    {
        //Überprüfung ob ProfilId, TagIds vorhanden sind und PreloadCount > 0 ist
        //Falls nicht -> leere Liste zurückgeben
        if (profileId <= 0 || tagIds is null || tagIds.Count == 0 || preloadCount <= 0)
            return Enumerable.Empty<HarvestUploadDto>();
        
        
        //Sicherheitscheck: falls keine alreadyRatedProfiles übergeben wurden
        alreadyRatedProfiles ??= new HashSet<int>();

        
        //Hier bin ich in einen bekannten EF Core Query Translation Bug gelaufen.
        //Anscheinend mag er die Kombi aus GroupBy, Select(g => g.OrderByDescending(...).First()), Select(new Dto) nicht.
        //-> KeyNotFoundException: EmptyProjectionMember 
        //Deswegen musste ich die Query "aufsplitten"
        var latestPerProfile = _dbContext.Harvestuploads
            //Da es sich hier um eine Read Only Abfrage handelt, muss sich EF kein Objekt merken
            .AsNoTracking()
            //Filter alle HarvestUploads des Content Receivers heraus
            .Where(h => h.Profileid != profileId)
            //Schließt alle Bilder aus von Profilen die bereits bewertet worden sind
            .Where(h => !alreadyRatedProfiles.Contains(h.Profileid))
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
                //Schließt alle Bilder aus von Profilen die bereits bewertet worden sind
                where !alreadyRatedProfiles.Contains(h.Profileid)
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
                    TagIds = h.Tags
                        .Select(t => t.Tagid)
                        .ToList(), 
                    ProfileId = h.Profileid
                })
            //PreloadCount gibt die Anzahl der benötigten DTOs an
            .Take(preloadCount)
            .ToList();

        return result;
    }
}