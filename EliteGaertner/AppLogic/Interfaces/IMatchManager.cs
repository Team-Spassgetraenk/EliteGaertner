using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;


//Das Interface implementiert die Match Logik beim Swipen der Profile
public interface IMatchManager
{
    
    
    //Diese Methode implementiert das Bewerten der Profile/Bilder.
    // //Der Content Receiver kann den User positiv oder negativ bewerten.
    // //Nach der Bewertung wird die jeweilige ProfileDTO mit ihrer HarvestDTO aus der Liste entfernt.
    // //Dabei wird immer überprüft ob noch genug Vorschläge in der Liste vorhanden sind.
    // //Falls nicht werden wieder neue Vorschläge generiert.
    public bool RateUser(int userId, int targetUserId, int value);


    public Dictionary<ProfileDto, HarvestUploadDto> CreateUserSuggestionList(int userId);

    public ProfileDto VisitUserProfile(int userId);

    public List<ProfileDto> ShowMatches(int userId);
    
    


}