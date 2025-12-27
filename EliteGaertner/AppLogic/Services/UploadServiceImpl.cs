using AppLogic.Interfaces;
using DataManagement.Interfaces;
using Contracts.Data_Transfer_Objects;

namespace AppLogic.Services;

public class UploadServiceImpl : IUploadService
{
    private readonly IHarvestDbs _harvestDbs;
    
    public UploadServiceImpl(IHarvestDbs harvestDbs)
    {
        _harvestDbs = harvestDbs;
    }
    
    public bool CreateHarvestUpload(HarvestUploadDto uploadDto)
    {
        var success = _harvestDbs.CreateUploadDbs(uploadDto);
        Console.WriteLine(success ? "Upload erfolgreich" : "Upload im ManagementDBS fehlgeschlagen"); return success;
    }
    
    
    public bool CreateHarvestUpload(int userId, string imageUrl, string description, float weight, int width, int length)
    {
        var uploadDto = new HarvestUploadDto
        {
            ProfileId = userId,
            ImageUrl = imageUrl,
            Description = description,
            WeightGram = weight,
            WidthCm = width,
            LengthCm = length,
            UploadDate = DateTime.UtcNow
        };
    
        bool success = _harvestDbs.CreateUploadDbs(uploadDto);
        Console.WriteLine(success ? "Upload erfolgreich" : "Upload im ManagementDBS fehlgeschlagen"); 
        return success;
    }
    

    public bool DeleteUpload(int uploadId, int userId)
    {
        throw new NotImplementedException();
    }

    public List<HarvestUploadDto> GetUserUploads(int userId)
    {
        throw new NotImplementedException();
    }
}