using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;

namespace DataManagement.Interfaces;


//TODO Kommentare fehlen
public interface IHarvestDbs
{

    //Gib mir die Harvestuploads des Users zurück.
    public IEnumerable<HarvestUploadDto> GetProfileHarvestUploads(int profileId);
    
    //Erstell mir eine Repository an HarvestUploadDtos die zum dem Interessensprofil
    //des Users passen. Die Menge der DTOs wird vom preloadCount bestimmt.
    public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(int profileId, List<int> tagIds, int preloadCount);
    
    public void CreateUploadDbs(HarvestUploadDto uploadDto);
    
    public HarvestUploadDto GetUploadDb(int uploadId);
    
    public void DeleteHarvestUpload(int uploadId);
    
    public void SetReportHarvestUpload(int uploadId, ReportReasons reason);
    
    public int GetReportCount(int uploadId);
}