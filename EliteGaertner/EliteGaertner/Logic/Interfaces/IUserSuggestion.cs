namespace EliteGaertner.Logic.Interfaces;

public interface IUserSuggestion
{

    //Diese Methode implementiert das Bewerten der Profile/Bilder.
    //Der Content Receiver kann den User positiv oder negativ bewerten.
    bool RateUser(int userId, int targetUserId, int value);
    
    
    



}