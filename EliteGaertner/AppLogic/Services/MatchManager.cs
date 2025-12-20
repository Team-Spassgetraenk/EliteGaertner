using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;
using DataManagement;

namespace AppLogic.Services;

public class MatchManager : IMatchManager
{
    private readonly ProfileDto _contentReceiver;
    private readonly PreferenceDto _preferenceDto;
    private readonly int _preloadCount;
    private readonly Dictionary<ProfileDto, HarvestUploadDto> _userSuggestionList;
    
    
    public MatchManager(ProfileDto contentReceiver, int preloadCount)
    {

        IPreferenceDbs preferenceDBS = new ManagementDbs();
        
        //Aufbereitung der ProfileId und der dazugehörigen TagIds
        var profileId = contentReceiver.ProfileId;
        var tagIds = contentReceiver.PreferenceDtos
            .Select(p => p.TagId)
            .Distinct()
            .ToList();

        _contentReceiver = contentReceiver;
        _preferenceDto = preferenceDBS.GetUserPreference(contentReceiver.UserId);
        _preloadCount = preloadCount;
        _userSuggestionList = CreateUserSuggestionList(_contentReceiver, _preferenceDto.Tags, _preloadCount);
    }

    public Dictionary<ProfileDto, HarvestUploadDto> CreateUserSuggestionList(ProfileDto contentReceiver, List<string> preferences, int preloadCount)
    {
     //   var userSuggestion = new UserSuggestion(contentReceiver, preferences, preloadCount);
     //   return userSuggestion.GetUserSuggestionList(contentReceiver.UserId);
    }

    public void AddSuggestions()
    {
        //Gib eine neue Liste an Suggestions zurück.
        var newSuggestions = CreateUserSuggestionList(_contentReceiver, _preferenceDto.Tags, _preloadCount);
        //Prüfe, ob die neu generierten Profile bereits in der "alten" Liste vorhanden sind.
        //Falls nicht, füge sie zur "alten" Liste hinzu.
        foreach (var sug in newSuggestions)
            if (!_userSuggestionList.ContainsKey(sug.Key))
                _userSuggestionList.Add(sug.Key, sug.Value);
    }

    public Dictionary<ProfileDto, HarvestUploadDto> GetUserSuggestionList()
        => _userSuggestionList;

    public void RateUser(ProfileDto targetProfile, bool value)
    {
        //Initialisiere die passende Datenbankschnittstelle.
        //Datenbank gibt das passende MatchDto zurück.
        IMatchesDbs matchesDbs = new ManagementDbs();
        MatchDto matchDto = matchesDbs.GetMatchInfo(_contentReceiver, targetProfile);

        //Speicher die Daten der MatchDto in den lokalen Variablen der Methode ab.
        var contentReceiver = matchDto.ContentReceiver;
        var contentReceiverValue = matchDto.ContentReceiverValue;
        var targetUser = matchDto.TargetProfile;
        var targetUserValue = matchDto.TargetProfileValue;

        //Überprüfe, ob der Content Receiver das targetProfile positiv oder negativ bewertet hat.
        //Falls sich beide User gegenseitig positiv bewertet haben, wird ein Match erstellt (CreateMatch).
        if (value == true)
        {
            contentReceiverValue = true;
            if (targetUserValue == true)
                CreateMatch(targetProfile);
        }

        //Erstell aus den lokalen Variablen ein neues MatchDto und speicher es in der Datenbank.
        var dto = new MatchDto
        {
            ContentReceiver = contentReceiver,
            ContentReceiverValue = contentReceiverValue,
            TargetProfile = targetUser,
            TargetProfileValue = targetUserValue
        };
        matchesDbs.SaveMatchInfo(dto);

        //Entferne den Vorschlag aus der Liste
        _userSuggestionList.Remove(targetProfile);

        //Falls, nicht mehr genug Suggestions in der Liste sind -> neue erstellen.
        if (_userSuggestionList.Count < 5)
            AddSuggestions();
    }

    
    //TODO Nicolas wird das implementieren. Wenn ich dazu komme, müssen wir uns absprechen.
    public void VisitUserProfile(ProfileDto targetProfile)
    {
        
        //Wir müssen auf die Datenbank zugreifen, um uns die passenden HarvestUploads zu holen.
        IHarvestDbs harvestDbs = new ManagementDbs();
        //Das Interface gibt eine Liste der HarvestUploads zurück.
        var harvestUploads = harvestDbs.GetHarvestUploadRepo(targetProfile);
    }
    
    
    public void CreateMatch(ProfileDto targetProfile)
    {
        //Initialisiere die passende Datenbankschnittstelle.
        //Datenbank gibt die passenden erfolgreichen Matches zurück.
        
        
    }
    
    public List<ProfileDto> ShowMatches()
    {
        IMatchesDbs matchesDbs = new ManagementDbs();
        return matchesDbs.GetSuccessfulMatches(_contentReceiver);
    }

}