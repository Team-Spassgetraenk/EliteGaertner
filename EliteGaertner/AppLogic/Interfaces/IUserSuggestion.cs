using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;


public interface IUserSuggestion
{
    
    
    //In dieser Methode übergeben wir die Liste mit den harvestSuggestions.
    //Anhand der enthaltenen Informationen können wir herausfinden welcher User für den Harvest-Upload
    //verantwortlich war. Für diesen User wird eine ProfileDTO erstellt. Die ProfileDto wird dann mit dem passenden 
    //Harvest-Upload in einer Dictionary abgelegt.
    void CreateUserSuggestions(List<HarvestUploadDto> harvestSuggestions);
    
    //Falls die User-Suggestions unter einem Schwellenwert fallen, dann
    //soll diese wieder aufgefüllt werden. Dabei lassen wir uns wieder eine Liste an Harvest-Suggestions
    //übergeben, die wir an die CreateUserSuggestions-Methode übergeben
    List<HarvestUploadDto> CreateHarvestSuggestions(int userId, List<string> preferences, int preloadCount);


    //Die Methode gibt eine Dictionary mit den passenden Usern und Harvestuploads zurück.
    Dictionary<ProfileDto, HarvestUploadDto> GetUserSuggestionList(int userId);
    
    //Hier wird überprüft ob der Content Receiver den Content Creator 
    //bereits bewertet hat.
    bool WasProfileShown(int userId, int targetUserId);

    //Hier übergeben wir die Liste der empfohlenen User mitsamt Uploads und eine Zahl (Prozent).
    //Die Methode überprüft ob noch genug User in der Liste sind. Das stellt er durch den
    //threshholdCount fest. z.B. bei 10 -> sind weniger als 10 Prozent von 
    
    
}