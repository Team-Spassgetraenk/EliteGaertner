using Contracts.Data_Transfer_Objects;

namespace AppLogic.Services;

public class MatchManager
{
    private readonly ProfileDto _contentReceiver;
    private readonly int _preloadCount;
    private IDictionary<ProfileDto, HarvestUploadDto> userSuggestionList;
    
    public MatchManager(ProfileDto contentReceiver, int preloadCount)
    {
        _contentReceiver = contentReceiver;
        _preloadCount = preloadCount;
        userSuggestionList = CreateUserSuggestionList(contentReceiver.UserId);

    }

    public Dictionary<ProfileDto, HarvestUploadDto> CreateUserSuggestionList(int userId)
    {
        
        
    }
    
    
}