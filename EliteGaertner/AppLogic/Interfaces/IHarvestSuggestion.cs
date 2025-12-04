using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;



//Das Interface ist für die HarvestSuggestionManagement Klasse zuständig.
//Sie beschreibt, welche Anforderungen an den Zugriff der einzelnen
//HarvestSuggestions besteht.
public interface IHarvestSuggestion
{
    
    //Gib die Liste der HarvestSuggestions zurück
    List<HarvestUploadDto> GetHarvestSuggestionList();
    
    
// Bin mir nicht sicher ob wir das überhaupt benötigen!!!    
//    //Gibt das DTO eines Harvest Uploads zurück.
//    HarvestUploadDto GetHarvest(int uploadId);
//    
//    //Gibt die URL des Erntebilds zurück.
//    string GetUrl(int uploadId);
//
//    
//    //Gibt die Beschreibung des Harvest-Uploads zurück.
//    string GetDescription(int uploadId);
//
//    
//    //Gibt das Gewicht des Harvest-Uploads zurück.
//    float GetWeight(int uploadId);
//
//    
//    //Gibt die Länge des Harvest-Uploads zurück.
//    float GetLength(int uploadId);
//
//    
//    //Gibt die Breite des Harvest-Uploads zurück.
//    float GetWidth(int uploadId);
//    
//    
//    //Gibt das Upload-Datum zurück.
//    DateTime GetDate(int uploadId);
}