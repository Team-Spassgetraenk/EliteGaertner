using System.Runtime.CompilerServices;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;

namespace DataManagement;

public class ManagementDbs : IHarvestDbs, IMatchesDbs, IPreferenceDbs, IProfileDbs
{
    public IList<HarvestUploadDto> GetHarvestUploadRepo(ProfileDto contentReceiver)
    {
        throw new NotImplementedException();
    }

    public IList<HarvestUploadDto> GetHarvestUploadRepo(ProfileDto contentReceiver, List<string> preferences, int preloadCount)
    {
        throw new NotImplementedException();
    }

    public MatchDto GetMatchInfo(ProfileDto contentReceiver, ProfileDto targetProfile)
    {
        throw new NotImplementedException();
    }

    public List<ProfileDto> GetSuccessfulMatches(ProfileDto contentReceiver)
    {
        throw new NotImplementedException();
    }

    public void SaveMatchInfo(MatchDto matchDto)
    {
        throw new NotImplementedException();
    }

    public PreferenceDto GetUserPreference(int userId)
    {
        throw new NotImplementedException();
    }

    public void SetUserPreference(int userId, PreferenceDto newUserPreference)
    {
        throw new NotImplementedException();
    }

    public ProfileDto GetProfile(int userId)
    {
        throw new NotImplementedException();
    }
}
