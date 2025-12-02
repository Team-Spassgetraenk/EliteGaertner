using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;


public interface IUserSuggestion
{
    
    
    //In dieser Methode übergeben wir die Liste mit den harvestSuggestions.
    //Anhand der enthaltenen Informationen können wir herausfinden welcher User für den Harvest-Upload
    //verantwortlich war. Für diesen User wird eine ProfileDTO erstellt. Die ProfileDto wird dann mit dem passenden 
    //Harvest-Upload in einer Dictionary abgelegt.
    Dictionary<ProfileDto, HarvestUploadDto> CreateUserSuggestions(List<HarvestUploadDto> harvestSuggestions);
    
    //Falls die User-Suggestions unter einem Schwellenwert fallen, dann
    //soll diese wieder aufgefüllt werden. Dabei lassen wir uns wieder eine Liste an Harvest-Suggestions
    //übergeben, die wir an die CreateUserSuggestions-Methode übergeben
    void CreateHarvestSuggestions(int userId, PreferenceDto userPreference);
    
    
    
    
    //Diese Methode implementiert das Bewerten der Profile/Bilder.
    //Der Content Receiver kann den User positiv oder negativ bewerten.
    //Nach der Bewertung wird die jeweilige ProfileDTO mit ihrer HarvestDTO aus der Liste entfernt.
    //Dabei wird immer überprüft ob noch genug Vorschläge in der Liste vorhanden sind.
    //Falls nicht werden wieder neue Vorschläge generiert.
    bool RateUser(int userId, int targetUserId, int value);

    
    //Diese Methode gibt eine IDictionary zurück, die die Liste der empfohlenen Harvestuploads
    //in HarvestSuggestion nimmt und diese den passenden Usern zuweist.
    //Diese IDictionary, wird dann genutzt um auf der Bewertungsseite, das Profil mitsamt dem passenden
    //Bild anzuzeigen.
    IDictionary<ProfileDto, HarvestUploadDto> ReturnRecommendedUserList(int userId, IList<HarvestUploadDto> harvestUploadDtos);


    //Hier wird überprüft ob der Content Receiver den Content Creator 
    //bereits bewertet hat.
    bool WasProfileShown(int userId, int targetUserId);

    //Hier übergeben wir die Liste der empfohlenen User mitsamt Uploads und eine Zahl (Prozent).
    //Die Methode überprüft ob noch genug User in der Liste sind. Das stellt er durch den
    //threshholdCount fest. z.B. bei 10 -> sind weniger als 10 Prozent von 
    
    bool RecommendedProfileCount(IDictionary<ProfilMgmDto, HarvestUploadDto> userList, int threshholdCount);
    
}