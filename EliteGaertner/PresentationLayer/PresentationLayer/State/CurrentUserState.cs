using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;

namespace PresentationLayer.State;

/// <summary>
/// Holds the currently logged-in user for the current Blazor Server circuit (Scoped).
/// 
/// - During login, call <see cref="LoginAs"/> with the PrivateProfileDto returned by AppLogic.
/// - After proper auth is implemented (claims/cookies), you can call <see cref="LoadByProfileIdAsync"/>
///   once you know the ProfileId from the authenticated principal.
/// </summary>
public sealed class CurrentUserState
{
    private readonly IProfileMgm _profileMgm;

    public CurrentUserState(IProfileMgm profileMgm)
    {
        _profileMgm = profileMgm;
    }

    public PrivateProfileDto? PrivateProfile { get; private set; }

    public int? ProfileId => PrivateProfile?.ProfileId;

    public bool IsLoggedIn => PrivateProfile != null;

    /// <summary>
    /// Use this right after a successful login/registration when you already have the DTO.
    /// </summary>
    public void LoginAs(PrivateProfileDto profile)
    {
        PrivateProfile = profile;
    }

    /// <summary>
    /// Use this when you only have the ProfileId (e.g., from authentication claims) and want to
    /// load the full PrivateProfileDto via AppLogic.
    /// </summary>
    public Task LoadByProfileIdAsync(int profileId)
    {
        PrivateProfile = _profileMgm.VisitPrivateProfile(profileId);
        return Task.CompletedTask;
    }

    public void Logout()
    {
        PrivateProfile = null;
    }
}