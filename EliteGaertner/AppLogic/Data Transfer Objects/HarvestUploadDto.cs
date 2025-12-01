namespace AppLogic.Logic.Data_Transfer_Objects;

public record HarvestUploadDto
{

    public int UploadId { get; init; }
    
    public int UserId { get; init; }
    public string ImageUrl { get; init; }
    public string Description { get; init; }
    public float WeightKg { get; init; }
    public float WidthCm { get; init; }
    public float LengthCm { get; init; }
    public int Quantity { get; init; }
    public DateTime UploadDate { get; init; }
    
}