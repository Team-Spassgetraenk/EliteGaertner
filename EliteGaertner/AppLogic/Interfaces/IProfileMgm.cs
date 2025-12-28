using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;

public interface IProfileMgm
{
    public bool CheckUsernameExists (string username);
    
    public PublicProfileDto VisitReceiverProfile(int userId); //Für Besucher

    public PrivateProfileDto VisitCreatorProfile(int userId); //Für das eigene

    public bool UpdateProfile(PrivateProfileDto profile);

    public bool UpdateContactVisibility(ContactVisibilityDto dto);
    
    public PrivateProfileDto RegisterProfile(PrivateProfileDto newProfile);

    public PrivateProfileDto LoginProfile(PrivateProfileDto receiverProfile);

    public List<PreferenceDto> GetPreference(int profileId);
    
    public bool SetPreference(List<PreferenceDto> preferences);
}