using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;

namespace AppLogic.Interfaces;

//Dieses Interface stellt alle Methoden zur Verfügung, die für die Erstellung und Verwaltung der Profil-
//und HarvestUpload Vorschläge benötigt werden. Zusätzlich sind hier die Methoden, die die Matchlogik behandeln.
public interface IMatchManager
{
    //Diese Methode erstellt für den Match Manager ein User Suggestion List.
    public Dictionary<PublicProfileDto, HarvestUploadDto> CreateProfileSuggestionList(int profileId, List<int> tagIds,
        int preloadCount);
    
    //Sobald in der User Suggestion List nicht mehr genug Vorschläge vorhanden sind, werden diese über die Methode nachgeladen
    public void AddSuggestions();
    
    //Gibt eine Liste an User+HarvestUpload Suggestions zurück
    public Dictionary<PublicProfileDto, HarvestUploadDto> GetProfileSuggestionList();
    
    //Diese Methode implementiert das Bewerten der Profile/Bilder.
    public void RateUser(PublicProfileDto targetProfile, bool value);
    
    //Diese Methode ist ein Event, die getriggert wird, sobald ein neues Match entsteht
    public event Action<PublicProfileDto>? CreateMatch;

    //Diese Methode übergibt dem PräsentationsLayer den nächsten Vorschlag aus der SuggestionList
    public (PublicProfileDto creator, HarvestUploadDto harvest)? GetNextSuggestion();
    
    //Nach jedem "RateUser" wird diese Methode aufgerufen, die die MatchListe aktualisiert und feststellt, ob neue
    //dazugekommen sind
    public List<PublicProfileDto> UpdateActiveMatches();

    //Übergibt die Liste an ActiveMatches an den Präsentations Layer
    public List<PublicProfileDto> GetActiveMatches();
    
    //Führt das ReportHandling für einen Vorschlag durch
    public void ReportHarvestUpload(int uploadId, ReportReasons reason);
}