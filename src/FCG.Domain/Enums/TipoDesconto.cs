using System.Text.Json.Serialization;

namespace FCG.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoDesconto
    {
        Moeda,
        Percentual
    }
}
