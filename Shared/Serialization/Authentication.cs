namespace Shared.Serialization
{
    /// <summary>
    /// Types of recieved messages enum ExchangeType
    /// </summary>
    public class Authentication : Request
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public byte[] PublicKey { get; set; }
        public byte[] IV { get; set; }
    }
}
