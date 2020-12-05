using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shared.Serialization
{
    public class Response
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("type")]
        public ExchangeType Type { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseCode Code { get; set; }
    }
}