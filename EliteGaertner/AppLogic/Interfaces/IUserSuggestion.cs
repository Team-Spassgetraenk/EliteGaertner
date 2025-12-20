using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;


public interface IUserSuggestion
{
    
    
    //In dieser Methode übergeben wir die Liste mit den harvestSuggestions.
    //Anhand der enthaltenen Informationen können wir herausfinden welcher User für den Harvest-Upload
    //verantwortlich war. Für diesen User wird eine ProfileDTO erstellt. Die ProfileDto wird dann mit dem passenden 
    //Harvest-Upload in einer Dictionary abgelegt.
    void CreateUserSuggestions(int profileId, List<HarvestUploadDto> harvestSuggestions);
    
    //Falls die User-Suggestions unter einem Schwellenwert fallen, dann
    //soll diese wieder aufgefüllt werden. Dabei lassen wir uns wieder eine Liste an Harvest-Suggestions
    //übergeben, die wir an die CreateUserSuggestions-Methode übergeben
    public List<HarvestUploadDto> CreateHarvestSuggestions(int profileId, List<int> tagIds, int preloadCount);
    
    //Die Methode gibt eine Dictionary mit den passenden Usern und Harvestuploads zurück.
    Dictionary<ProfileDto, HarvestUploadDto> GetUserSuggestionList(int userId);
    
    //Hier wird überprüft ob der Content Receiver den Content Creator 
    //bereits bewertet hat.
    bool ProfileAlreadyRated (int profileIdReceiver,  int profileIdCreator);
    
    
    
    
}