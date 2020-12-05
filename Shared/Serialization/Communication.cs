using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shared.Serialization
{
    public class Communication : Request
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CommunicationType CommunicationType { get; set; }
        public string UserTo { get; set; }
        public byte[] Message { get; set; }
    }
}
