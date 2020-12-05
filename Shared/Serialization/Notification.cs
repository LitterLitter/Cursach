using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shared.Serialization
{
    public class Notification : Response
    {
        public byte[] PublicKey { get; set; }
        public byte[] IV { get; set; }
        public string Login { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationCode NotificationType { get; set; }
    }
}
