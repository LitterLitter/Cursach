using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Shared.Serialization
{
    public class ResponseConverter : JsonConverter<Response>
    {
        public override Response ReadJson(JsonReader reader, Type objectType, Response existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jObj = JObject.Load(reader);
            var responseType = jObj["type"].ToObject<ExchangeType>();
            return responseType switch
            {
                ExchangeType.Authentication => jObj.ToObject<AuthResponse>(),
                ExchangeType.Registration => jObj.ToObject<RegResponse>(),
                ExchangeType.Communication => jObj.ToObject<CommunicationResponse>(),
                ExchangeType.Notification => jObj.ToObject<Notification>(),
                ExchangeType.Handshake=>jObj.ToObject<HandshakeResponse>(),
                _ => throw new InvalidCastException("Received missing response")
            };
        }

        public override void WriteJson(JsonWriter writer, Response value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
