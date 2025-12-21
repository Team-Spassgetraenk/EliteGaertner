using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;
using DataManagement;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        UpdateActiveMatches();
    }

    public Dictionary<PublicProfileDto, HarvestUploadDto> CreateProfileSuggestionList(int profileId, List<int> tagIds, int preloadCount)
    { 
        //Initialisiere die ProfileSuggestion Klasse
        var profileSuggestion = new ProfileSuggestion(_matchesDbs, _profileDbs, _harvestDbs, profileId, _tagIds, preloadCount);
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
            if (!_profileSuggestionList.ContainsKey(sug.Key))
                _profileSuggestionList.Add(sug.Key, sug.Value);
    }

    public Dictionary<PublicProfileDto, HarvestUploadDto> GetProfileSuggestionList()
        => _profileSuggestionList;

    
    public void RateUser(PublicProfileDto creatorProfile, bool value)
    {
        //Holt sich MatchDto aus Datenbank
        var matchDto = _matchesDbs.GetMatchInfo(_profileId, creatorProfile.ProfileId); 
        
        //Erstelle neue MatchDto mit der Bewertung und Zeitpunkt
        var dto = new MatchDto
        {
            ContentReceiver = _profileId,
            ContentReceiverValue = value,
            ContentCreator = matchDto.ContentCreator,
            ContentCreatorValue = matchDto.ContentCreatorValue,
            ContentReceiverRatingDate = DateTime.Now,
            ContentCreatorRatingDate = matchDto.ContentCreatorRatingDate
        };
        _matchesDbs.SaveMatchInfo(dto);
        
        //Entferne Profil + HarvestUpload aus der Liste
        _profileSuggestionList.Remove(creatorProfile);
        
        //Erstelle ein Match, wenn sich beide Profile positiv bewertet haben
        if (dto.ContentCreatorValue && dto.ContentReceiverValue)
            CreateMatch(creatorProfile);

        //Nach jeder Bewertung wird die Matchliste aktualisiert
        UpdateActiveMatches();

        //Falls, nicht mehr genug Suggestions in der Liste sind -> neue erstellen.
        if (_profileSuggestionList.Count < 5)
            AddSuggestions();
    }

    public PublicProfileDto CreateMatch(PublicProfileDto creatorProfile)
        => creatorProfile;
    
    public void UpdateActiveMatches()
    {
        _activeMatchesList = _matchesDbs.GetActiveMatches(_profileId);
    }

    public List<PublicProfileDto> GetActiveMatches()
        => _activeMatchesList;
}