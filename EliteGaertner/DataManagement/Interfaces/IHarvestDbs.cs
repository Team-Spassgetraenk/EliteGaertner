using Contracts.Data_Transfer_Objects;

namespace DataManagement.Interfaces;


//Dieses Interface implementiert
public interface IHarvestDbs
{

    //Gib mir die Harvestuploads des Users zurück.
    public IEnumerable<HarvestUploadDto> GetProfileHarvestUploads(int profileId);
    
    //Erstell mir eine Repository an HarvestUploadDtos die zum dem Interessensprofil
    //des Users passen. Die Menge der DTOs wird vom preloadCount bestimmt.
    public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(int profileId, List<int> tagIds, int preloadCount);

    //TODO Nicolas
    public void SetHarvestUpload(HarvestUploadDto harvestUpload);
    
    //TODO Nicolas
    public void DeleteHarvestUpload(int uploadId);
    
    //TODO
    public void SetReportHarvestUpload(int uploadId, Enum reason);

    //TODO
    public IEnumerable<ReportDto> GetReportHarvestUpload(int uploadId);
}