namespace Contracts.Data_Transfer_Objects;

//TODO
public record MatchManagerDto
{
    public int ProfileId { get; init; }
    public List<int> TagIds { get; init; }
    public int PreloadCount { get; init; }
    public Dictionary<PublicProfileDto, HarvestUploadDto> ProfileSuggestionList { get; init; }
    public List<PublicProfileDto> ActiveMatchesList { get; init; }
    
}