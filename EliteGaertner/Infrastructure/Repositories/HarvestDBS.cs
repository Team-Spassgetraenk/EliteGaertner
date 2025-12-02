using Infrastructure.Interfaces;
using Contracts.Data_Transfer_Objects;

namespace Infrastructure.Repositories;

public class HarvestDBS : IHarvestDBS
{
    private IList<HarvestUploadDto> HarvestRepository = new List<HarvestUploadDto>();


    public IList<HarvestUploadDto> GetHarvestUploadRepo(int userId)
    {
        throw new NotImplementedException();
    }

    public IList<HarvestUploadDto> GetHarvestUploadRepo(int userId, int preloadCount, PreferenceDto userPreference)
    {
        throw new NotImplementedException();
    }
}