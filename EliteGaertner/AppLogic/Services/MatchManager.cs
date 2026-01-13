using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;
using DataManagement.Interfaces;
using Microsoft.Extensions.Logging;

namespace AppLogic.Services;

public class MatchManager : IMatchManager
{
    private readonly IMatchesDbs _matchesDbs;
    private readonly IProfileDbs _profileDbs;
    private readonly IHarvestDbs _harvestDbs;
    private readonly ILogger<MatchManager> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly int _profileId;
    private readonly List<int> _tagIds;
    private readonly Dictionary<PublicProfileDto, HarvestUploadDto> _profileSuggestionList;
    private List<PublicProfileDto> _activeMatchesList;
    private const int ReportThreshold = 5;
    private const int PreloadCount = 10;
    
    public MatchManager(IMatchesDbs matchesDbs, IProfileDbs profileDbs, IHarvestDbs harvestDbs,
        PrivateProfileDto contentReceiver, ILoggerFactory loggerFactory)
    {
        _matchesDbs = matchesDbs;
        _profileDbs = profileDbs;
        _harvestDbs = harvestDbs;
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.CreateLogger<MatchManager>();
        
        //Aufbereitung der ProfileId
        _profileId = contentReceiver.ProfileId;
        _logger.LogInformation("MatchManager initialized. ContentReceiverProfileId={ProfileId}", _profileId);
        
        //Aufbereitung der TagIds
        _tagIds = contentReceiver.PreferenceDtos
            .Select(p => p.TagId)
            .Distinct()
            .ToList();
        _profileSuggestionList = CreateProfileSuggestionList(_profileId, _tagIds, PreloadCount);
        
        _logger.LogInformation("Initial suggestions loaded. Count={SuggestionCount}, PreloadCount={PreloadCount}, TagIdsCount={TagIdsCount}",
            _profileSuggestionList.Count, PreloadCount, _tagIds.Count);
       
        //activeMatchesList wird initialisiert und dann mit den aktuellen Matches befüllt
        _activeMatchesList = new List<PublicProfileDto>();
        _activeMatchesList = _matchesDbs.GetActiveMatches(_profileId).ToList();
        
        _logger.LogInformation("Active matches loaded. Count={ActiveMatchesCount}", _activeMatchesList.Count);
    }

    public Dictionary<PublicProfileDto, HarvestUploadDto> CreateProfileSuggestionList(int profileId, List<int> tagIds, int preloadCount)
    {
        _logger.LogDebug("Creating profile suggestions. ProfileId={ProfileId}, TagIdsCount={TagIdsCount}, PreloadCount={PreloadCount}",
            profileId, tagIds?.Count ?? 0, preloadCount);

        //Logger für Harvest- und ProfileSuggestion erstellen
        var profileLogger = _loggerFactory.CreateLogger<ProfileSuggestion>();
        var harvestLogger = _loggerFactory.CreateLogger<HarvestSuggestion>();
        
        //Initialisiere die ProfileSuggestion Klasse
        var profileSuggestion = new ProfileSuggestion(_matchesDbs, _profileDbs, _harvestDbs, profileId, tagIds, preloadCount, profileLogger, harvestLogger);
        //Gib die Profil + HarvestUpload Vorschläge zurück 
        var result = profileSuggestion.GetProfileSuggestionList();
        
        _logger.LogDebug("Profile suggestions created. Count={SuggestionCount}", result.Count);
        
        return result;
    }

    public void AddSuggestions()
    {
        //Gib eine neue Liste an Suggestions zurück.
        var newSuggestions = CreateProfileSuggestionList(_profileId, _tagIds, PreloadCount);
        
        _logger.LogDebug("AddSuggestions called. NewSuggestionsCount={NewSuggestionsCount}, ExistingCount={ExistingCount}",
            newSuggestions.Count, _profileSuggestionList.Count);
        
        //Prüfe, ob die neu generierten Profile bereits in der "alten" Liste vorhanden sind.
        //Falls nicht, füge sie zur "alten" Liste hinzu.
        foreach (var sug in newSuggestions)
        {
            var alreadyExists = _profileSuggestionList.Keys.Any(k => k.ProfileId == sug.Key.ProfileId);
            if (!alreadyExists)
                _profileSuggestionList.Add(sug.Key, sug.Value);
        }
        _logger.LogInformation("Suggestions merged. TotalCount={TotalCount}", _profileSuggestionList.Count);
    }

    public Dictionary<PublicProfileDto, HarvestUploadDto> GetProfileSuggestionList()
        => _profileSuggestionList;
    
    public void RateUser(PublicProfileDto creatorProfile, bool value)
    {
        try
        {
            _logger.LogInformation("RateUser called. ContentReceiver={ContentReceiver}, ContentCreator={ContentCreator}, Value={Value}",
                _profileId, creatorProfile.ProfileId, value);
            
            //Erstelle neue MatchDto mit der Bewertung und Zeitpunkt
            var dto = new RateDto
            {
                ContentReceiver = _profileId,
                ContentCreator = creatorProfile.ProfileId,
                ContentReceiverValue = value,
                ContentReceiverRatingDate = DateTime.UtcNow,
            };
            
            //Speicher Bewertung ab
            _matchesDbs.SaveRateInfo(dto);
            
            _logger.LogDebug("Rating saved. ContentReceiver={ContentReceiver}, ContentCreator={ContentCreator}", _profileId, creatorProfile.ProfileId);
        
            //Entferne Profil + HarvestUpload aus der Liste
            var keyToRemove = _profileSuggestionList.Keys
                .SingleOrDefault(p => p.ProfileId == creatorProfile.ProfileId);
            if (keyToRemove != null)
                _profileSuggestionList.Remove(keyToRemove);
            
            _logger.LogDebug("Suggestion removed (if existed). RemainingSuggestionsCount={RemainingSuggestionsCount}", _profileSuggestionList.Count);
        
            //Nach jeder Bewertung wird die Matchliste aktualisiert
            _activeMatchesList = UpdateActiveMatches();

            //Falls, nicht mehr genug Suggestions in der Liste sind -> neue erstellen.
            if (_profileSuggestionList.Count < 5)
            {
                _logger.LogInformation("Low suggestion count detected. Remaining={Remaining}. Triggering AddSuggestions().", _profileSuggestionList.Count);
                
                AddSuggestions();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RateUser failed. ContentReceiver={ContentReceiver}, ContentCreator={ContentCreator}, Value={Value}",
                _profileId, creatorProfile.ProfileId, value);
            
            throw new InvalidOperationException(
            $"Fehler beim Bewerten eines Users. ContentReceiver={_profileId}, " +
            $"ContentCreator={creatorProfile.ProfileId}, " +
            $"Value={value}",
            ex
            );
        }
        
    }

    public event Action<PublicProfileDto>? CreateMatch;
    
    public List<PublicProfileDto> UpdateActiveMatches()
    {
        //Holt sich die aktuellen Aktiven Matches aus der Datenbank
        var newActiveMatchesList = _matchesDbs.GetActiveMatches(_profileId).ToList();
        
        _logger.LogDebug("UpdateActiveMatches called. PreviousCount={PreviousCount}, NewCount={NewCount}",
            _activeMatchesList.Count, newActiveMatchesList.Count);

        //Herausfinden welche neuen Matches dazu gekommen sind
        var newlyAddedMatches =
            newActiveMatchesList
                .Where(newProfile =>
                    //Gib mir alle Elemente der _activeMatchesList
                    _activeMatchesList.All(existing =>
                        //und gib mir alle Elemente zurück, die nicht die selben ProfilIds haben -> also neu sind
                        existing.ProfileId != newProfile.ProfileId))
                .ToList();
        
        if (newlyAddedMatches.Count > 0)
            _logger.LogInformation("New matches detected. Count={NewMatchesCount}", newlyAddedMatches.Count);
        
        //Für jedes neue Match wird die CreateMatch Methode aufgerufen
        foreach (var newMatch in newlyAddedMatches)
        {
            //Solange keiner den CreateMatch subscribed (passiert im Presentation Layer) 
            //bleibt CreateMatch Null. Deswegen wird CreateMatch nur ausgeführt, wenn er nicht null ist 
            _logger.LogDebug("Raising CreateMatch event for ProfileId={ProfileId}", newMatch.ProfileId);
            
            CreateMatch?.Invoke(newMatch);
        }
        
        //_activeMatchesList wird durch die neue aktualisierte ActiveMatches Liste ersetzt
        _activeMatchesList = newActiveMatchesList;
        return _activeMatchesList;
    }

    public List<PublicProfileDto> GetActiveMatches()
        => _activeMatchesList;

    public (PublicProfileDto creator, HarvestUploadDto harvest)? GetNextSuggestion()
    {
        //Prüfe, ob ein Vorschlag vorhanden ist
        if (_profileSuggestionList.Count == 0)
        {
            _logger.LogDebug("GetNextSuggestion: no suggestions available.");
            
            return null;
        }
        
        //Nimm das erste Paar aus den Vorschlägen
        var first = _profileSuggestionList.First();
        
        _logger.LogDebug("GetNextSuggestion returned. CreatorProfileId={CreatorProfileId}, UploadId={UploadId}",
            first.Key.ProfileId, first.Value.UploadId);
        
        //Gib Key und Value aus dem einzelnen Vorschlag zurück 
        return (first.Key, first.Value);
    }
    
    public void ReportHarvestUpload(int uploadId, ReportReasons reason)
    {
        try
        {
            _logger.LogInformation("ReportHarvestUpload called. UploadId={UploadId}, Reason={Reason}", uploadId, reason);
            
            //HarvestUpload reporten
            _harvestDbs.SetReportHarvestUpload(uploadId,reason);
            
            //Gibt zurück wie oft das Bild bereits reportet worden ist
            var reportCount = _harvestDbs.GetReportCount(uploadId);
            
            _logger.LogInformation("Report saved. UploadId={UploadId}, ReportCount={ReportCount}", uploadId, reportCount);
            
            //Sobald ein Bild 5 Mal reported worden ist, wird es gelöscht
            if (reportCount >= ReportThreshold)
            {
                _logger.LogWarning("Report threshold reached. Deleting upload. UploadId={UploadId}, ReportCount={ReportCount}, Threshold={Threshold}",
                    uploadId, reportCount, ReportThreshold);
                
                _harvestDbs.DeleteHarvestUpload(uploadId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ReportHarvestUpload failed. UploadId={UploadId}, Reason={Reason}", uploadId, reason);
            
            throw new InvalidOperationException(
                $"Fehler beim Melden von UploadId={uploadId} (Reason={reason}).", ex);
        }
    }
}