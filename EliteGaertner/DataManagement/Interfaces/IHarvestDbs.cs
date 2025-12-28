using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;

namespace DataManagement.Interfaces;


//Dieses Interface implementiert
public interface IHarvestDbs
{

    //Gib mir die Harvestuploads des Users zurück.
    public IEnumerable<HarvestUploadDto> GetProfileHarvestUploads(int profileId);
    
    //Erstell mir eine Repository an HarvestUploadDtos die zum dem Interessensprofil
    //des Users passen. Die Menge der DTOs wird vom preloadCount bestimmt.
    public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(int profileId, List<int> tagIds, int preloadCount);
    
    public bool CreateUploadDbs(HarvestUploadDto uploadDto);
    
    public HarvestUploadDto GetUploadDb(int uploadId);
    
    //TODO Nicolas
    public bool DeleteHarvestUpload(int uploadId);
    
    //TODO
    public void SetReportHarvestUpload(int uploadId, ReportReasons reason);

    //TODO
    public int GetReportCount(int uploadId);
}