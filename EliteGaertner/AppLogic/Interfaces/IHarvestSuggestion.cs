using AppLogic.Logic.Data_Transfer_Objects;

namespace AppLogic.Logic.Interfaces;



//Das Interface ist für die HarvestSuggestion Klasse zuständig.
//Sie ist dafür zuständig anhand dem Interessensprofil des Users die passenden 
//Harvest-Uploads aus der Datenbank zurückzugeben.
public interface IHarvestSuggestion
{

    //Diese Methode gibt uns eine Liste mit HarvestUploads zurück.
    //Wir möchten schon die passenden HarvestUploads buffern, damit die 
    //Performance beim bewerten der Profile/Bilder flüssig bleibt.
    IList<HarvestUploadDto> GetNextSuggestion(int userId, int count);

    
    //Gibt das DTO eines Harvest Uploads zurück.
    HarvestUploadDto GetHarvest(int uploadId);

    
    //Gibt die URL des Erntebilds zurück.
    string GetUrl(int uploadId);

    
    //Gibt die Beschreibung des Harvest-Uploads zurück.
    string GetDescription(int uploadId);

    
    //Gibt das Gewicht des Harvest-Uploads zurück.
    float GetWeight(int uploadId);

    
    //Gibt die Länge des Harvest-Uploads zurück.
    float GetLength(int uploadId);

    
    //Gibt die Breite des Harvest-Uploads zurück.
    float GetWidth(int uploadId);
    
    
    //Gibt das Upload-Datum zurück.
    DateTime GetDate(int uploadId);
}