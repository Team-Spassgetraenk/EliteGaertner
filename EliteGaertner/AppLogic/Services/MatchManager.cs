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
    private Dictionary<ProfileDto, HarvestUploadDto> userSuggestionList;
    
    //Maybe ProfilMgmDto löschen? Für was brauchen wir das?
    //Könnten einfach mit einem ProfileDto arbeiten.
    public MatchManager(ProfileDto contentReceiver, int preloadCount)
    {

        IPreferenceDbs preferenceDBS = new ManagementDbs();
        
        _contentReceiver = contentReceiver;
        _preferenceDto = preferenceDBS.GetUserPreference(contentReceiver.UserId);
        _preloadCount = preloadCount;
        userSuggestionList = CreateUserSuggestionList(_contentReceiver, _preferenceDto.Tags, _preloadCount);
    }
    
    public Dictionary<ProfileDto, HarvestUploadDto> CreateUserSuggestionList(ProfileDto contentReceiver, List<String> preferences, int preloadCount)
    {
        var userSuggestion = new UserSuggestion(contentReceiver, preferences, preloadCount);
        return userSuggestion.GetUserSuggestionList(userId);
    }

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
            TargetUserValue = targetUserValue
        };
        matchesDbs.SaveMatchInfo(dto);

        //Entferne den Vorschlag aus der Liste
        userSuggestionList.Remove(targetProfile);

        //Falls, nicht mehr genug Suggestions in der Liste sind -> neue erstellen.
        if (userSuggestionList.Count < 5)
            AddSuggestions();
    }

    private void AddSuggestions()
    {
        //Gib eine neue Liste an Suggestions zurück.
        var newSuggestions = CreateUserSuggestionList(_contentReceiver.UserId, _preferenceDto.Tags, _preloadCount);
        //Prüfe, ob die neu generierten Profile bereits in der "alten" Liste vorhanden sind.
        //Falls nicht, füge sie zur "alten" Liste hinzu.
        foreach (var sug in newSuggestions)
            if (!userSuggestionList.ContainsKey(sug.Key))
                userSuggestionList.Add(sug.Key, sug.Value); 
    }


    public void VisitUserProfile(int userId)
    {
        
        
        
        
    }
    
    //????????Noch nicht fertig
    public void CreateMatch(ProfileDto targetProfile)
    {
        //Initialisiere die passende Datenbankschnittstelle.
        //Datenbank gibt die passenden erfolgreichen Matches zurück.
        IMatchesDBS matchesDbs = new ManagementDBS();
        var successfulMatches = matchesDbs.GetSuccessfulMatches(targetProfile);
        
        
        


    }
}