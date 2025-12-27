using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;

public interface IProfileMgm
{
    public PrivateProfileDto VisitReceiverProfile(int userId);

    public PrivateProfileDto VisitCreatorProfile(int userId);

    public bool UpdateProfile(PrivateProfileDto profile);

    public bool SetContactVisibility(ContactVisibilityDto contactVisibility);

    //TODO
    public PrivateProfileDto RegisterProfile(PrivateProfileDto newProfile);

    //TODO
    public PrivateProfileDto LoginProfile(PrivateProfileDto receiverProfile);
    
    //TODO
    public List<PreferenceDto> SetPreferences(PreferenceDto profilePreferences);
}