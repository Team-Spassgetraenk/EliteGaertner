using Contracts.Data_Transfer_Objects;

namespace PresentationLayer.State;

public sealed class PreferenceState
{
    //Das ist das HashSet, dass die aktuellen TagIds des CurrentProfils besitzt
    private readonly HashSet<int> _currentProfileTagIds = new();
    //Getter für die Liste
    public IReadOnlyCollection<int> CurrentProfileTagIds => _currentProfileTagIds;
    
    public void InitializeCurrentProfileTagIds(IReadOnlyCollection<PreferenceDto> prefDtos)
    {
        //Liste zurücksetzen
        _currentProfileTagIds.Clear();
        
        //Überprüfung
        if (prefDtos is null)
            return;
        
        //Speicher die TagIds aus den Dtos in der Liste ab
        foreach (var dto in prefDtos)
            _currentProfileTagIds.Add(dto.TagId);
    }

    //Fügt oder entfernt eine TagId aus der Liste
    public void ToggleTag(int tagId)
    {
        //Versuch tagId zur Liste hinzuzufügen
        if (!_currentProfileTagIds.Add(tagId))
            //-> Falls nicht möglich ist -> Aus der Liste entfernen, da es bereits vorhanden ist
            _currentProfileTagIds.Remove(tagId);
    }

    //TODO KOMMENTAR FEHLT
    public List<PreferenceDto> CreatePreferenceDtos(int profileId)
    {
        List<PreferenceDto> result = new();
        
        foreach (var tagId in _currentProfileTagIds)
        {
            result.Add(new PreferenceDto
            {
                TagId = tagId,
                Profileid = profileId,
                DateUpdated = DateTime.UtcNow
            });
        }

        return result;
    }
}