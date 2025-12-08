using System.Runtime.InteropServices.JavaScript;

namespace DataManagement.Entities;

public class Matches
{
    public int MatchId { get; set; }
    public int UserId1 { get; set; }
    public int UserId2 { get; set; }
    public DateTime MatchActiveDate { get; set; }
    public bool matchActive { get; set; }
}