namespace DataManagement.Entities;

public class Rating
{
    public int ContentCreator { get; set; }
    public int ContentReceiver { get; set; }
    public bool ProfileRating { get; set; }
    public DateTime RatingDate { get; set; }
    public DateTime MatchActiveDate { get; set; }
}