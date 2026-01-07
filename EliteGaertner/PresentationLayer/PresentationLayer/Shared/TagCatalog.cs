using System.Collections.Generic;
using System.Linq;

namespace PresentationLayer.Shared;

public static class TagCatalog
{
    public sealed record TagItem(int TagId, string Name, string Icon);
    
    public static readonly IReadOnlyList<TagItem> Gemuese = new List<TagItem>
    {
        new(1,  "Auberginen", "🍆"),
        new(2,  "Gurken",     "🥒"),
        new(3,  "Tomaten",    "🍅"),
        new(4,  "Kürbisse",   "🎃"),
        new(5,  "Paprika",    "🌶️"),
        new(6,  "Zucchini",   "🥒"),
        new(7,  "Kartoffeln", "🥔"),
        new(8,  "Karotten",   "🥕"),
        new(9,  "Salate",     "🥬"),
        new(10, "Zwiebeln",   "🧅"),
        new(18, "Bohnen",     "🫘"),
        new(19, "Spinat",     "🥬"),
        new(20, "Radieschen", "🌱"), 
        new(21, "Brokkoli",   "🥦"),
        new(22, "Mais",       "🌽"),
    };

    public static readonly IReadOnlyList<TagItem> Obst = new List<TagItem>
    {
        new(11, "Melonen",   "🍉"),
        new(12, "Äpfel",     "🍎"),
        new(13, "Birnen",    "🍐"),
        new(14, "Pfirsiche", "🍑"),
        new(15, "Kirschen",  "🍒"),
        new(16, "Erdbeeren", "🍓"),
        new(17, "Trauben",   "🍇"),
    };
    
    //Gibt die vollständige Liste aus Gemüse und Obst TagItems zurück
    private static readonly IReadOnlyList<TagItem> _all =
        Gemuese.Concat(Obst).ToList();

    //Gibt dir anhand der TagId das passende Tag zurück
    //Benötigen wir um die TagIds aus der Datenbank mit den Tags auf der UI zu mappen
    private static readonly Dictionary<int, TagItem> _byId =
        _all.ToDictionary(t => t.TagId, t => t);

    //Gibt die passenden TagItems anhand des Namens zurück
    private static readonly Dictionary<string, TagItem> _byName =
        _all.ToDictionary(t => t.Name, t => t);
    
    //Diese Methoden rufen die einzelnen Field ab
    //Die Liste ändert sich während der Programmlaufzeit nicht, also werden die benötigten Listen 
    //in den Fields einmal bei Programmstart initialisiert und dann nur noch von den Methoden bei Bedarf abgefragt
    public static TagItem? FindById(int tagId)
        => _byId.TryGetValue(tagId, out var item) ? item : null;

    public static TagItem? FindByName(string? name)
        => name != null && _byName.TryGetValue(name, out var item) ? item : null;

    public static IReadOnlyList<TagItem> All => _all;
}