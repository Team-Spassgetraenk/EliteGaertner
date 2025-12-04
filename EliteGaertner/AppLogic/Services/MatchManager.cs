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
    private Dictionary<ProfileDto, HarvestUploadDto> userSuggestionList;
    
    
    
    //Maybe ProfilMgmDto löschen? Für was brauchen wir das?
    //Könnten einfach mit einem ProfileDto arbeiten.
    public MatchManager(ProfileDto contentReceiver, int preloadCount)
    {

        IPreferenceDBS preferenceDBS = new ManagementDBS();
        
        _contentReceiver = contentReceiver;
        _preferenceDto = preferenceDBS.GetUserPreference(contentReceiver.UserId);
        _preloadCount = preloadCount;
        userSuggestionList = CreateUserSuggestionList(_contentReceiver.UserId, _preferenceDto.Tags, _preloadCount);
    }

    public Dictionary<ProfileDto, HarvestUploadDto> CreateUserSuggestionList(int userId, List<String> preferences, int preloadCount)
    {
        var userSuggestion = new UserSuggestion(userId, preferences, preloadCount);
        return userSuggestion.GetUserSuggestionList(userId);
    }

    public void RateUser(ProfileDto targetProfile, bool value)
    {
        IMatchesDBS matchesDbs = new ManagementDBS();
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
            ContentReceiver = contentReceiver,
            ContentReceiverValue = contentReceiverValue,
            TargetUser = targetUser,
            TargetUserValue = targetUserValue
        };
        matchesDbs.SaveMatchInfo(dto);

        userSuggestionList.Remove(targetProfile);

        if (userSuggestionList.Count < 5)
        {
            userSuggestionList.AddRange(CreateUserSuggestionList(_contentReceiver.UserId, _preferenceDto.Tags, _preloadCount));
        }
    }

    public void CreateMatch(ProfileDto targetProfile)
    {
        
        
    }
    
    
}