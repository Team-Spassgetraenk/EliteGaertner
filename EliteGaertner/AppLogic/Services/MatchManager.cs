using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace AppLogic.Services;

public class MatchManager : IMatchManager
{
    private readonly ProfileDto _contentReceiver;
    private readonly PreferenceDto _preferenceDto;
    private readonly int _preloadCount;
    private IDictionary<ProfileDto, HarvestUploadDto> userSuggestionList;
    
    
    
    //Maybe ProfilMgmDto löschen? Für was brauchen wir das?
    //Könnten einfach mit einem ProfileDto arbeiten.
    public MatchManager(ProfileDto contentReceiver, int preloadCount)
    {
        _contentReceiver = contentReceiver;
        _preloadCount = preloadCount;
        userSuggestionList = CreateUserSuggestionList(contentReceiver.UserId, preloadCount);
    }

    public Dictionary<ProfileDto, HarvestUploadDto> CreateUserSuggestionList(int userId, int preloadCount)
    {
        var userSuggestion = new UserSuggestion(userId, preloadCount);
        return userSuggestion.GetUserSuggestionList(userId);
    }

    public void RateUser(ProfileDto targetProfile, bool value)
    {
        IMatchesDBS matchesDbs = new MatchesDBS();
        MatchDto matchDto = matchesDbs.GetMatchDto(_contentReceiver, targetProfile);

        var contentReceiver = matchDto.ContentReceiver;
        var contentReceiverValue = matchDto.ContentReceiverValue;

        var targetUser = matchDto.TargetUser;
        var targetUserValue = matchDto.TargetUserValue;
        
        if (value == true)
        {
            contentReceiverValue = true;
            if (targetUserValue == true)
                CreateMatch(targetProfile);
        }
        
        var dto = new MatchDto
        {
            ContentReceiver = _contentReceiver,
            ContentReceiverValue = contentReceiverValue,
            TargetUser = targetProfile,
            TargetUserValue = targetUserValue
        };
        matchesDbs.SaveMatchInfo(dto);
    }

    public void CreateMatch(ProfileDto targetProfile)
    {
        
        
    }
    
    
}