namespace Infrastructure.Repositories.Entities;

public class HarvestUploads
{
    public int UploadId { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public float WeightKg { get; set; }
    public float WidthCm { get; set; }
    public float LengthCm { get; set; }
    public DateTime UploadDate { get; set; }
    public int ProfileId { get; set; }
}