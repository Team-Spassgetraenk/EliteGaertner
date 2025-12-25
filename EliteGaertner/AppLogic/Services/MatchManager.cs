using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;
using DataManagement.Interfaces;

namespace AppLogic.Services;

public class MatchManager : IMatchManager
{
    private readonly IMatchesDbs _matchesDbs;
    private readonly IProfileDbs _profileDbs;
    private readonly IHarvestDbs _harvestDbs;
    private readonly int _profileId;
    private readonly List<int> _tagIds;
    private readonly int _preloadCount;
    private readonly Dictionary<PublicProfileDto, HarvestUploadDto> _profileSuggestionList;
    private List<PublicProfileDto> _activeMatchesList;
    
    public MatchManager(IMatchesDbs matchesDbs, IProfileDbs profileDbs, IHarvestDbs harvestDbs, 
        PrivateProfileDto contentReceiver, int preloadCount)
    {
        _matchesDbs = matchesDbs;
        _profileDbs = profileDbs;
        _harvestDbs = harvestDbs;
        
        //Aufbereitung der ProfileId und der dazugehörigen TagIds
        _profileId = contentReceiver.ProfileId;
        _tagIds = contentReceiver.PreferenceDtos
            .Select(p => p.TagId)
            .Distinct()
            .ToList();
        _preloadCount = preloadCount;
        _profileSuggestionList = CreateProfileSuggestionList(_profileId, _tagIds, _preloadCount);
        
        //Hier müssen wir die _activeMatchesList erstmal initialisieren bevor wir
        //die Ergebnisse aus der UpdateActiveMatches übergeben können, da er in der Methode überprüft,
        //ob _activeMatchesList leer ist oder nicht. -> NullReferenceException
        _activeMatchesList = new List<PublicProfileDto>();
        _activeMatchesList = UpdateActiveMatches();
    }

    public Dictionary<PublicProfileDto, HarvestUploadDto> CreateProfileSuggestionList(int profileId, List<int> tagIds, int preloadCount)
    { 
        //Initialisiere die ProfileSuggestion Klasse
        var profileSuggestion = new ProfileSuggestion(_matchesDbs, _profileDbs, _harvestDbs, profileId, tagIds, preloadCount);
        //Gib die Profil + HarvestUpload Vorschläge zurück 
        return profileSuggestion.GetProfileSuggestionList();
    }

    public void AddSuggestions()
    {
        //Gib eine neue Liste an Suggestions zurück.
        var newSuggestions = CreateProfileSuggestionList(_profileId, _tagIds, _preloadCount);
        //Prüfe, ob die neu generierten Profile bereits in der "alten" Liste vorhanden sind.
        //Falls nicht, füge sie zur "alten" Liste hinzu.
        foreach (var sug in newSuggestions)
        {
            var alreadyExists = _profileSuggestionList.Keys.Any(k => k.ProfileId == sug.Key.ProfileId);
            if (!alreadyExists)
                _profileSuggestionList.Add(sug.Key, sug.Value);
        }
    }

    public Dictionary<PublicProfileDto, HarvestUploadDto> GetProfileSuggestionList()
        => _profileSuggestionList;
    
    public void RateUser(PublicProfileDto creatorProfile, bool value)
    {
        
        //Erstelle neue MatchDto mit der Bewertung und Zeitpunkt
        var dto = new RateDto
        {
            ContentReceiver = _profileId,
            ContentCreator = creatorProfile.ProfileId,
            ContentReceiverValue = value,
            ContentReceiverRatingDate = DateTime.UtcNow,
        };
        _matchesDbs.SaveMatchInfo(dto);
        
        //Entferne Profil + HarvestUpload aus der Liste
        var keyToRemove = _profileSuggestionList.Keys
            .SingleOrDefault(p => p.ProfileId == creatorProfile.ProfileId);
        if (keyToRemove != null)
            _profileSuggestionList.Remove(keyToRemove);
        
        //Nach jeder Bewertung wird die Matchliste aktualisiert
        _activeMatchesList = UpdateActiveMatches();

        //Falls, nicht mehr genug Suggestions in der Liste sind -> neue erstellen.
        if (_profileSuggestionList.Count < 5)
            AddSuggestions();
    }

    public PublicProfileDto CreateMatch(PublicProfileDto creatorProfile)
        => creatorProfile;
    
    public List<PublicProfileDto> UpdateActiveMatches()
    {
        var newActiveMatchesList = _matchesDbs.GetActiveMatches(_profileId).ToList();
        
        //Falls die _activeMatchesList null ist, schreibt er den Rückgabewert der Datebank sofort rein
        if (_activeMatchesList.Count == 0)
        {
            _activeMatchesList = newActiveMatchesList;
            return _activeMatchesList;
        }

        //Herausfinden welche neuen Matches dazu gekommen sind
        var newlyAddedMatches =
            newActiveMatchesList
                .Where(newProfile =>
                    //Gib mir alle Elemente der _activeMatchesList
                    _activeMatchesList.All(existing =>
                        //und gib mir alle Elemente zurück, die nicht die selben ProfilIds haben -> also neu sind
                        existing.ProfileId != newProfile.ProfileId))
                .ToList();
        
        //Für jedes neue Match wird die CreateMatch Methode aufgerufen
        foreach (var newMatch in newlyAddedMatches)
        {
            CreateMatch(newMatch);
        }
        
        _activeMatchesList = newActiveMatchesList;
        return _activeMatchesList;
    }

    public List<PublicProfileDto> GetActiveMatches()
        => _activeMatchesList;

    //TODO IMPLEMENTIERUNG FEHLT
    public void ReportHarvestUpload(int uploadId, ReportReasons reason)
    {
        
    }
    
    public MatchManagerDto GetMatchManager()
        => new MatchManagerDto
        {
            ProfileId = _profileId,
            TagIds = _tagIds,
            PreloadCount = _preloadCount,
            ProfileSuggestionList = _profileSuggestionList,
            ActiveMatchesList = _activeMatchesList
        };
}