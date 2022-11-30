using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace DraCraft.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TierType
    {
        T1,
        T2,
        T3,
        T4,
        T5
    }
}
