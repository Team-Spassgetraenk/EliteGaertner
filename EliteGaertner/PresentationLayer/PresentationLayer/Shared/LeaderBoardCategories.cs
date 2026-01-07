using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;

namespace PresentationLayer.Shared;

public static class LeaderBoardCategories
{

    public static readonly IReadOnlyList<LeaderboardDto> Leaderboards = new List<LeaderboardDto>
    {
        new()
        {
            LeaderboardTitle = "Meisten Likes 💚",
            LeaderboardId = 1,
            TagId = null,
            Goal = LeaderboardSearchGoal.MostLikes,
            PersonalEntry = null,
            Entries = null
        },
        
        new()
        {
            LeaderboardTitle = "Längste Gurke 🥒",
            LeaderboardId = 2,
            TagId = TagCatalog.FindByName("Gurken").TagId,
            Goal = LeaderboardSearchGoal.Longest,
            PersonalEntry = null,
            Entries = null  
        },
        
        new()
        {
            LeaderboardTitle = "Schwerste Aubergine 🍆",
            LeaderboardId = 3,
            TagId = TagCatalog.FindByName("Auberginen").TagId,
            Goal = LeaderboardSearchGoal.Heaviest,
            PersonalEntry = null,
            Entries = null
        }
    };
}