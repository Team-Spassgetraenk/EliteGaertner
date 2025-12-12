namespace DataManagement.Entities;

public class Report
{
    public int ReportId { get; set; }
    public string Reason { get; set; }
    public DateTime ReportDate { get; set; }
    public int UploadId { get; set; }
}