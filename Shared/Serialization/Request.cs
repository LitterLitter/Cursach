using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shared.Serialization
{
    public class Request
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("type")]
        public ExchangeType Type { get; set; }
    }
}
