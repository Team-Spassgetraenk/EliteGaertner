namespace DataManagement.Entities;

public class Rating
{
    public int ContentCreatorId { get; set; }
    public int ContentReceiverId { get; set; }
    public bool ProfileRating { get; set; }
    public DateTime RatingDate { get; set; }
    public DateTime MatchActiveDate { get; set; }
}