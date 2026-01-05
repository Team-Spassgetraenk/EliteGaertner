using Contracts.Data_Transfer_Objects;

namespace PresentationLayer.State;

public sealed class CurrentProfileState
{
    public PrivateProfileDto? LoggedInProfile { get; private set; }
    public bool IsLoggedIn => LoggedInProfile != null;
    public int? ProfileId => LoggedInProfile?.ProfileId;
    public string? ProfilepictureUrl => LoggedInProfile?.ProfilepictureUrl;
    public string? UserName => LoggedInProfile?.UserName;
    public string? FirstName => LoggedInProfile?.FirstName;
    public string? LastName => LoggedInProfile?.LastName;
    public string? EMail => LoggedInProfile?.EMail;
    public string? Phonenumber => LoggedInProfile?.Phonenumber;
    public string? Profiletext => LoggedInProfile?.Profiletext;
    public bool? ShareMail => LoggedInProfile?.ShareMail;
    public bool? SharePhoneNumber => LoggedInProfile?.SharePhoneNumber;
    public DateTime? UserCreated => LoggedInProfile?.UserCreated;
    public IReadOnlyList<HarvestUploadDto>? HarvestUploads => LoggedInProfile?.HarvestUploads;
    public IReadOnlyList<PreferenceDto>? PreferenceDtos => LoggedInProfile?.PreferenceDtos;
    
    //TODO KOMMT WEG -> DEBUGGEN
    public Guid InstanceId { get; } = Guid.NewGuid();

    public void Login(PrivateProfileDto profile)
    {
        LoggedInProfile = profile;
    }

    public void Logout()
    {
        LoggedInProfile = null;
    }
}