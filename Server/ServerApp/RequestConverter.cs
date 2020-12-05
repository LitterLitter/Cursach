using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Serialization;

namespace Server
{
    public class RequestConverter : JsonConverter<Request>
    {
        public override Request ReadJson(JsonReader reader, Type objectType, Request existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var Jobj = JObject.Load(reader);
            var pact = Jobj["type"].ToObject<ExchangeType>();
            return pact switch
            {
                ExchangeType.Authentication => Jobj.ToObject<Authentication>(),
                ExchangeType.Registration => Jobj.ToObject<Registration>(),
                ExchangeType.Communication => Jobj.ToObject<Communication>(),
                ExchangeType.Handshake => Jobj.ToObject<HandshakeRequest>(),
                ExchangeType.EncryptedData => Jobj.ToObject<EncryptedDataWrapper>(),
                
                _ => throw new InvalidOperationException("Received invalid package type")
            };
        }

        public override void WriteJson(JsonWriter writer, Request value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
