namespace PresentationLayer.Shared;

public static class CountryCodeCatalog
{
    public sealed record CountryCode(string Name, string Code, string Flag);

    public static IReadOnlyList<CountryCode> CountryCodes { get; } = new List<CountryCode>
    {
        new("Deutschland", "+49", "🇩🇪"),
        new("Österreich", "+43", "🇦🇹"),
        new("Schweiz", "+41", "🇨🇭"),
        new("Vereinigtes Königreich", "+44", "🇬🇧"),
        new("Vereinigte Staaten (USA)", "+1", "🇺🇸"),
        new("Kanada", "+1", "🇨🇦"),
        new("Belgien", "+32", "🇧🇪"),
        new("Bulgarien", "+359", "🇧🇬"),
        new("Dänemark", "+45", "🇩🇰"),
        new("Estland", "+372", "🇪🇪"),
        new("Finnland", "+358", "🇫🇮"),
        new("Frankreich", "+33", "🇫🇷"),
        new("Griechenland", "+30", "🇬🇷"),
        new("Irland", "+353", "🇮🇪"),
        new("Italien", "+39", "🇮🇹"),
        new("Kroatien", "+385", "🇭🇷"),
        new("Lettland", "+371", "🇱🇻"),
        new("Litauen", "+370", "🇱🇹"),
        new("Luxemburg", "+352", "🇱🇺"),
        new("Malta", "+356", "🇲🇹"),
        new("Niederlande", "+31", "🇳🇱"),
        new("Polen", "+48", "🇵🇱"),
        new("Portugal", "+351", "🇵🇹"),
        new("Rumänien", "+40", "🇷🇴"),
        new("Schweden", "+46", "🇸🇪"),
        new("Slowakei", "+421", "🇸🇰"),
        new("Slowenien", "+386", "🇸🇮"),
        new("Spanien", "+34", "🇪🇸"),
        new("Tschechien", "+420", "🇨🇿"),
        new("Ungarn", "+36", "🇭🇺"),
        new("Zypern", "+357", "🇨🇾"),
        new("Andorra", "+376", "🇦🇩"),
        new("Island", "+354", "🇮🇸"),
        new("Norwegen", "+47", "🇳🇴"),
        new("Türkei", "+90", "🇹🇷"),
        new("Australien", "+61", "🇦🇺"),
        new("Neuseeland", "+64", "🇳🇿"),
        new("Japan", "+81", "🇯🇵"),
        new("China", "+86", "🇨🇳"),
        new("Mexiko", "+52", "🇲🇽"),
        new("Brasilien", "+55", "🇧🇷"),
    };
}