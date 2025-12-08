using Contracts.Data_Transfer_Objects;

namespace DataManagement.Interfaces;


//Dieses Interface implementiert
public interface IHarvestDbs
{

    //Gib mir die Harvestuploads des Users zurück.
    public IList<HarvestUploadDto> GetHarvestUploadRepo(int userId);
    
    //Erstell mir eine Repository an HarvestUploadDtos die zum dem Interessensprofil
    //des Users passen. Die Menge der DTOs wird vom preloadCount bestimmt.
    public IList<HarvestUploadDto> GetHarvestUploadRepo(int userId, List<string> preferences, int preloadCount);
    
    

}