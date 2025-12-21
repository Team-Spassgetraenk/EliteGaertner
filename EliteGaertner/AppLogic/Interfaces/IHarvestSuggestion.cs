using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;

//Dieses Interface stellt alle Methoden zur Verfügung, die für die Erstellung der HarvestUpload-Vorschläge 
//benötigt werden
public interface IHarvestSuggestion
{
    //Gib die Liste der HarvestSuggestions zurück
    List<HarvestUploadDto> GetHarvestSuggestionList();
}