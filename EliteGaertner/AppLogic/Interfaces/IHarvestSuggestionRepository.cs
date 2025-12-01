using AppLogic.Logic.Data_Transfer_Objects;

namespace AppLogic.Logic.Interfaces;


//Das Interface beschreibt die Implementierung der Harvest Suggestion Repository in 
//der HarvestSuggestionManagement Klasse. Hier wird beschrieben welche Funktionen der Datensatz
//der HarvestSuggestions bereitstellen muss. Aus den Interessen des Users werden passende
//HarvestSuggestions von der Datenbank zurückgegeben und in Klasse abgespeichert.
//Wir buffern auch somit die passenden HarvestSuggestions vor, damit die Performance
//beim Bewerten stabil bleibt.
public interface IHarvestSuggestionRepository
{

    public HarvestUploadDto? GetById(int uploadId);

}