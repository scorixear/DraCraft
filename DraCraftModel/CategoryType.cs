using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DraCraft.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CategoryType
    {
        Schmelzen,
        Angeln,
        Lederverarbeitung,
        Reagenzien,
        Herstellungsmods,
        Juwelenschleiferei,
        Farbstoffe,
        Rohstoffe,
        Steinmetzkunst,
        Stimmkugeln,
        Holzverarbeitung,
        Weberei,
        Arkana,
        Kochkunst,
        Sonstiges,
        Waffen,
        Kleidung,
        Möbel
    }
}
