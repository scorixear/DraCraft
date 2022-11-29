using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DraCraftModel
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
        Kleidung
    }
}
