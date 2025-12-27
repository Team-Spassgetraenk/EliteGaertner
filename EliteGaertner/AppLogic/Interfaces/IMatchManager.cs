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
    
    //TODO kommentar fehlt
    public void AddSuggestions();
    
    //Gibt eine Liste an User+HarvestUpload Suggestions zurück
    public Dictionary<PublicProfileDto, HarvestUploadDto> GetProfileSuggestionList();
    
    //Diese Methode implementiert das Bewerten der Profile/Bilder.
    //Der Content Receiver kann den User positiv oder negativ bewerten.
    //Nach der Bewertung wird die jeweilige ProfileDTO mit ihrer HarvestDTO aus der Liste entfernt.
    //Dabei wird auch überprüft, ob es zu einem Match kommt. Falls ja -> CreateMatch Methode.
    //Es wird dabei immer überprüft, ob noch genug Vorschläge in der Liste vorhanden sind.
    //Falls nicht, werden wieder neue Vorschläge generiert.
    public void RateUser(PublicProfileDto targetProfile, bool value);
    
    //TODO KOMMENTAR FEHLT
    public PublicProfileDto CreateMatch(PublicProfileDto targetProfile);
    
    //TODO Kommentar fehlt
    public List<PublicProfileDto> UpdateActiveMatches();

    //TODO KOMMENTAR FEHLT
    public List<PublicProfileDto> GetActiveMatches();
    
    //TODO KOMMENTAR FEHLT
    public void ReportHarvestUpload(int uploadId, ReportReasons reason);
    
    //TODO METHODE FEHLT!!
    public MatchManagerDto GetMatchManager();

}