using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;


public interface IUserSuggestion
{

    //Diese Methode implementiert das Bewerten der Profile/Bilder.
    //Der Content Receiver kann den User positiv oder negativ bewerten.
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