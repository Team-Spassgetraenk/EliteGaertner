using AppLogic.Logic.Data_Transfer_Objects;
using AppLogic.Logic.Interfaces;

namespace AppLogic.Logic.Services;

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