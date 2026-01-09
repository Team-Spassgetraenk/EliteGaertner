namespace PresentationLayer.State;

/// <summary>
/// Hält den kompletten Registrierungszustand über mehrere Seiten hinweg.
/// Wird erst am Ende in DTOs gemappt und persistiert.
/// </summary>
public sealed class UserRegistrationState
{
    //Seite ProfilDatenRegistrierung
    public string UserName { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public bool ShareEmail { get; set; }
    public bool SharePhoneNumber { get; set; }
    public string PlainPassword { get; set; } = ""; 
    public string ConfirmPassword { get; set; } = "";

    //Seite ProfilBildRegistrierung
    public string ProfilePictureUrl { get; set; } = "";
    public string ProfileText { get; set; } = "";

    //Seite PräferenzAuswahlRegistrierung
    public HashSet<int> SelectedTagIdsPreferences { get; } = new();
    
    //Seite ErnteHochladenPrivat
    public string ImageUrl { get; set; } = "";
    public string Description { get; set; } = "";
    public float? WeightGram { get; set; }
    public float? WidthCm { get; set; }
    public float? LengthCm { get; set; }
    public HashSet<int> SelectedTagIdsHarvestUpload { get; } = new();
    
    //Resettet nach erfolgreichem Speichern den UserRegisterState
    public void Reset()
    {
        //Seite ProfilDatenRegistrierung
        UserName = "";
        FirstName = "";
        LastName = "";
        Email = "";
        PlainPassword = "";
        ConfirmPassword = "";
        PhoneNumber = "";
        ShareEmail = false;
        SharePhoneNumber = false;

        //Seite ProfilBildRegistrierung
        ProfilePictureUrl = "";
        ProfileText = "";
        
        //SeitePräferenzAuswahlRegistrierung
        SelectedTagIdsPreferences.Clear();
        
        //Seite ErnteHochladenPrivat
        ImageUrl = "";
        Description = "";
        WeightGram = null;
        WidthCm = null;
        LengthCm = null;
        SelectedTagIdsHarvestUpload.Clear();
    }
    
    //Step Validierungen: Pro Seite überprüft er ob alle Werte gesetzt worden sind
    //Validation: Seite ProfilDatenRegistrierung
    public bool IsStepProfilDataValid=>
        !string.IsNullOrWhiteSpace(UserName) &&
        !string.IsNullOrWhiteSpace(FirstName) &&
        !string.IsNullOrWhiteSpace(LastName) &&
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(PlainPassword) &&
        PlainPassword == ConfirmPassword;
    
    //Validation: Seite ProfilBildRegistrierung
    public bool IsStepProfilPictureValid =>
        !string.IsNullOrWhiteSpace(ProfileText) &&
        !string.IsNullOrWhiteSpace(ProfilePictureUrl);
    
    //Validation: Seite PräferenzAuswahlRegistrierung
    public bool IsStepPreferenceValid =>
        SelectedTagIdsPreferences.Count > 0;
    
    //Validation: Seite ErnteHochladenPrivat
    public bool IsStepHarvestUploadValid=>
        !string.IsNullOrWhiteSpace(ImageUrl) &&
        !string.IsNullOrWhiteSpace(Description) &&
        WeightGram.HasValue &&
        WidthCm.HasValue &&
        LengthCm.HasValue &&
        SelectedTagIdsHarvestUpload.Count > 0;

    public bool IsReadyForRegistration =>
        IsStepProfilDataValid &&
        IsStepProfilPictureValid &&
        IsStepPreferenceValid &&
        IsStepHarvestUploadValid &&
        SelectedTagIdsHarvestUpload.Count > 0 &&
        SelectedTagIdsPreferences.Count > 0;
}