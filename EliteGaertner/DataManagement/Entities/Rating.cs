namespace DataManagement.Entities;

public class Rating
{
    public int ProfileId1 { get; set; }
    public int ProfileId2 { get; set; }
    public bool ProfileRating { get; set; }
    public DateTime RateDate { get; set; }
    public int MatchId { get; set; }
}