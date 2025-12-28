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

    public HarvestUploadDto GetUploadDto(int uploadId)
    {
       return _harvestDbs.GetUploadDb(uploadId);
    }

    public string DeleteUpload(int uploadId, int userId) //Hier wird null zurückgegeben, wenn nicht existiert löschung
                                                         // des Bildes muss architekturwegens in .razor abgehandelt werden
    {
        var uploadDto = GetUploadDto(uploadId);
        
        if (uploadDto?.ImageUrl.Length <= 0 )
        {
            Console.WriteLine("Upload oder ImageUrl fehlt");
            return null;
        }

        var fileName = uploadDto.ImageUrl;
        
        _harvestDbs.DeleteHarvestUpload(uploadId);

        return fileName;
    }

    public List<HarvestUploadDto> GetUserUploads(int userId)
    {
        throw new NotImplementedException();
    }
}