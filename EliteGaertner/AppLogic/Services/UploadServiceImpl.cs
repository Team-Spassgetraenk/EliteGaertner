using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;

namespace AppLogic.Services;

public class UploadServiceImpl : IUploadService
{
    public bool CreateUpload(int userId, string imageUrl, string description, float weight, int quantity, DateTime uploadDate)
    {
        throw new NotImplementedException();
    }

    public bool CreateUpload(HarvestUploadDto uploadDto)
    {
        throw new NotImplementedException();
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