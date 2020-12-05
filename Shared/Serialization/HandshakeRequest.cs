
namespace Shared.Serialization
{
    public class HandshakeRequest:Request
    {
        public byte[] ClientKey { get; set; }
        public byte[] iv { get; set; }
    }
}
