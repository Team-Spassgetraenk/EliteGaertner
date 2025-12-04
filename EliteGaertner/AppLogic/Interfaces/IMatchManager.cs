using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;


//Das Interface implementiert die Match Logik beim Swipen der Profile
public interface IMatchManager
{
    
    
    //Diese Methode implementiert das Bewerten der Profile/Bilder.
    //Der Content Receiver kann den User positiv oder negativ bewerten.
    //Nach der Bewertung wird die jeweilige ProfileDTO mit ihrer HarvestDTO aus der Liste entfernt.
    //Dabei wird auch überprüft ob es zu einem Match kommt. Falls ja -> CreateMatch Methode
    //Dabei wird immer überprüft ob noch genug Vorschläge in der Liste vorhanden sind.
    //Falls nicht werden wieder neue Vorschläge generiert.
    public void RateUser(ProfileDto targetProfile, bool value);

    
    //Diese Methode ist für die 
    public void CreateMatch(ProfileDto targetProfile);


    //Diese Methode erstellt für den Match Manager ein User Suggestion List.
    public Dictionary<ProfileDto, HarvestUploadDto> CreateUserSuggestionList(int userId, int preloadCount);

    public ProfileDto VisitUserProfile(int userId);

    
    public List<ProfileDto> ShowMatches(int userId);
    
    


}