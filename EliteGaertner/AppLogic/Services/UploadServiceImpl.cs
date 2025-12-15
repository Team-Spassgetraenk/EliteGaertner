using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;


namespace AppLogic.Services;

public class UploadServiceImpl : IUploadService
{
    
    public bool CreateUpload(int userId, string imageUrl, string description, float weight, int quantity, DateTime uploadDate)
    {
        Console.WriteLine("Upload angekommen.");
        return true;
    }

    public bool CreateUpload(HarvestUploadDto uploadDto)
    {
        Console.WriteLine("Upload angekommen.");
        return true;
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