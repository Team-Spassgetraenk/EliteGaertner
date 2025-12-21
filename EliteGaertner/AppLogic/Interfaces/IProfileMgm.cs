using Contracts.Data_Transfer_Objects;

namespace AppLogic.Interfaces;

public interface IProfileMgm
{
    
    PrivateProfileDto VisitReceiverProfile(int userId);

    PrivateProfileDto VisitCreatorProfile(int userId);

    bool UpdateProfile(PrivateProfileDto profile);

    bool SetContactVisibility(ContactVisibilityDto contactVisibility);
}