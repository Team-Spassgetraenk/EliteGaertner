namespace Contracts.Data_Transfer_Objects;

public record PreferenceDto
{
    public List<string> Tags { get; init; }
}