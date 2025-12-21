using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;

namespace Tests.UnitTests.AppLogicTests.FakeDbs;

public class FakeHarvestDbs : IHarvestDbs
{
    public IEnumerable<HarvestUploadDto> GetHarvestUploadsRepo(int profileId)
    {
        throw new NotImplementedException();
    }


    public IEnumerable<HarvestUploadDto> GetProfileHarvestUploads(int profileId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(int profileId, List<int> tagIds, int preloadCount)
    {
        return new List<HarvestUploadDto>
        {
            new HarvestUploadDto { UploadId = 1, ProfileId = 2 },
            new HarvestUploadDto { UploadId = 2, ProfileId = 3 },
            new HarvestUploadDto { UploadId = 3, ProfileId = 4 }
        };
    }

    public void SetReportHarvestUpload(int uploadId, Enum reason)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ReportDto> GetReportHarvestUpload(int uploadId)
    {
        throw new NotImplementedException();
    }
}