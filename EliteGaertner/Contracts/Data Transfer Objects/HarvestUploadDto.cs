namespace Contracts.Data_Transfer_Objects;

public record HarvestUploadDto
{
    public int UploadId { get; init; }
    public string ImageUrl { get; init; }
    public string Description { get; init; }
    public float WeightGram { get; init; }
    public float WidthCm { get; init; }
    public float LengthCm { get; init; }
    public DateTime UploadDate { get; init; }
    public int ProfileId { get; init; }
    
}