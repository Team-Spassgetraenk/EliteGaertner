using Contracts.Data_Transfer_Objects;
using AppLogic.Interfaces;

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
    
    //Action die alle Seiten abbonieren, die Daten speichern/löschen müssen 
    //Diese Action triggert einen Render Neustart auf der jeweiligen Seite
    public event Action? Onchange;
    private void NotifyStateChanged()
    {
        Onchange?.Invoke();
    }

    public void Login(PrivateProfileDto profile)
    {
        LoggedInProfile = profile;
    }
    
    public void RefreshProfile(IProfileMgm profileMgm)
    {
        //Prüft ob ProfilId vorhanden ist
        if (ProfileId is null)
            return;

        //Lädt aus der Datenbank das PrivateProfileDto neu
        LoggedInProfile = profileMgm.GetPrivProfile(ProfileId.Value);
        
        //Trigget den Render Neustart
        NotifyStateChanged();
    }

    public void Logout()
    {
        LoggedInProfile = null;
    }
}