using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;
using DataManagement.Interfaces;
using DataManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataManagement;

public class HarvestDbs : IHarvestDbs
{
    private readonly EliteGaertnerDbContext _dbContext;
    private readonly ILogger<HarvestDbs> _logger;
    
    public HarvestDbs(EliteGaertnerDbContext dbContext, ILogger<HarvestDbs> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }   
    
    public IEnumerable<HarvestUploadDto> GetProfileHarvestUploads(int profileId)
    {
        _logger.LogInformation("DB: GetProfileHarvestUploads start. profileId={ProfileId}", profileId);
        
        //Falls die profileId <= 0 ist, return leere HarvestUploadDto Enumerable 
        if (profileId <= 0)
        {
            _logger.LogWarning("DB: GetProfileHarvestUploads invalid profileId. profileId={ProfileId}", profileId);
            return Enumerable.Empty<HarvestUploadDto>();
        }
        
        //Gib mir alle HarvestUploads
        var result = _dbContext.Harvestuploads
            .AsNoTracking()
            //Die der ProfileId zugeordnet sind
            .Where(h => h.Profileid == profileId)
            //Erstell aus den Ergebnissen neue HarvestUploadDtos
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
        _logger.LogDebug("DB: GetProfileHarvestUploads query built. profileId={ProfileId}", profileId);

        return result;
    }
        
    public void CreateUploadDbs(HarvestUploadDto uploadDto)
    {
        _logger.LogInformation("DB: CreateUploadDbs start. profileId={ProfileId}, tagIdsCount={TagIdsCount}", uploadDto?.ProfileId ?? 0, uploadDto?.TagIds?.Count ?? 0);
        
        //Prüfungen
        if (uploadDto.ProfileId <= 0)
            throw new ArgumentException("Ungültige ProfileId");

        if (uploadDto.TagIds == null || uploadDto.TagIds.Count == 0)
            throw new ArgumentException("Mindestens ein Tag muss angegeben werden.");
        
        try
        {
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
            
            _logger.LogInformation("DB: CreateUploadDbs done. profileId={ProfileId}, savedUploadImageUrl={ImageUrl}", uploadDto.ProfileId, uploadDto.ImageUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DB: CreateUploadDbs failed. profileId={ProfileId}", uploadDto?.ProfileId ?? 0);
            throw;
        }
    }

    public HarvestUploadDto GetHarvestUploadDto(int uploadId)
    {
        _logger.LogInformation("DB: GetHarvestUploadDto start. uploadId={UploadId}", uploadId);
        
        if (uploadId <= 0)
        {
            _logger.LogWarning("DB: GetHarvestUploadDto invalid uploadId. uploadId={UploadId}", uploadId);
            return new HarvestUploadDto();
        }
        try
        {
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

            _logger.LogInformation("DB: GetHarvestUploadDto done. uploadId={UploadId}, found={Found}", uploadId, dto is not null);
            return dto ?? new HarvestUploadDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DB: GetHarvestUploadDto failed. uploadId={UploadId}", uploadId);
            throw;
        }
    }
    
    public void DeleteHarvestUpload(int uploadId)
    {
        _logger.LogInformation("DB: DeleteHarvestUpload start. uploadId={UploadId}", uploadId);
        
        //Prüfungen
        if (uploadId <= 0)
            throw new ArgumentOutOfRangeException(nameof (uploadId), "UploadId muss größer als 0 sein.");
        try
        {
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
            
            _logger.LogInformation("DB: DeleteHarvestUpload done. uploadId={UploadId}", uploadId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DB: DeleteHarvestUpload failed. uploadId={UploadId}", uploadId);
            throw;
        }
    }
    
    public void SetReportHarvestUpload(int uploadId, ReportReasons reason)
    {
        _logger.LogInformation("DB: SetReportHarvestUpload start. uploadId={UploadId}, reason={Reason}", uploadId, reason);
        if (uploadId <= 0)
            throw new ArgumentOutOfRangeException(nameof (uploadId), "UploadId muss größer als 0 sein.");
        try
        {
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
            
            _logger.LogInformation("DB: SetReportHarvestUpload done. uploadId={UploadId}, reason={Reason}", uploadId, reason);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DB: SetReportHarvestUpload failed. uploadId={UploadId}, reason={Reason}", uploadId, reason);
            throw;
        }
    }
    
    public int GetReportCount(int uploadId)
    {
        _logger.LogDebug("DB: GetReportCount start. uploadId={UploadId}", uploadId);
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
        var count = _dbContext.Reports
            .AsNoTracking()
            .Count(r => r.Uploadid == uploadId);

        _logger.LogInformation("DB: GetReportCount done. uploadId={UploadId}, count={Count}", uploadId, count);

        return count;
    }
    
    public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(int profileId, List<int> tagIds, HashSet<int> alreadyRatedProfiles, int preloadCount)
    {
        _logger.LogInformation("DB: GetHarvestUploadRepo start. profileId={ProfileId}, tagIdsCount={TagIdsCount}, alreadyRatedCount={AlreadyRatedCount}, preloadCount={PreloadCount}",
            profileId, tagIds?.Count ?? 0, alreadyRatedProfiles?.Count ?? 0, preloadCount);
        
        //Überprüfung ob ProfilId, TagIds vorhanden sind und PreloadCount > 0 ist
        //Falls nicht -> leere Liste zurückgeben
        if (profileId <= 0 || tagIds is null || tagIds.Count == 0 || preloadCount <= 0)
        {
            _logger.LogWarning("DB: GetHarvestUploadRepo invalid input. profileId={ProfileId}, tagIdsCount={TagIdsCount}, preloadCount={PreloadCount}",
                profileId, tagIds?.Count ?? 0, preloadCount);
            return Enumerable.Empty<HarvestUploadDto>();
        }
        
        try
        {
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

            _logger.LogInformation("DB: GetHarvestUploadRepo done. profileId={ProfileId}, resultCount={ResultCount}", profileId, result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DB: GetHarvestUploadRepo failed. profileId={ProfileId}", profileId);
            throw;
        }
    }
}