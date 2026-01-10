using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;

//Dieses Interface stellt alle Methoden zur Verfügung, die für die Erstellung und Verwaltung der Profilvorschläge 
//benötigt werden
public interface IProfileSuggestion
{
    //ProfilDtos und HarvestUploadDtos werden hier zu einer kompletten Vorschlagsliste zusammengesetzt
    public void CreateProfileSuggestions(int profileId, List<HarvestUploadDto> harvestSuggestions);
    
    //Hier werden die passenden HarvestUploadDto Vorschläge erstellt
    public List<HarvestUploadDto> CreateHarvestSuggestions(int profileId, List<int> tagIds, int preloadCount);
    
    //Die Methode gibt die vollständige Vorschlagsliste zurück
    public Dictionary<PublicProfileDto, HarvestUploadDto> GetProfileSuggestionList();

    //Diese Methode fragt ab, welche Profile bereits vom ContentReceiver bewertet worden sind
    public void LoadAlreadyRatedProfiles(int profileId);
}