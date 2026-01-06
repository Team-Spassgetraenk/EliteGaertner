using Microsoft.AspNetCore.Components;
using PresentationLayer.State;

namespace PresentationLayer.Services;

//Die Klasse regelt den Logout
public sealed class SessionService
{
    //Klasse benötigt den CurrentProfileState + Nav
    private readonly CurrentProfileState _currentProfileState;
    private readonly NavigationManager _nav;

    public SessionService(CurrentProfileState currentProfileState, NavigationManager nav)
    {
        _currentProfileState = currentProfileState;
        _nav = nav;
    }

    public void Logout()
    {
        //Führt den Logout aus 
        _currentProfileState.Logout();
        //Navigiert zur Startseite zurück
        _nav.NavigateTo("/", forceLoad: true);
    }
}