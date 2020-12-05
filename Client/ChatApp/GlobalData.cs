using Client.App.Integration;
using System.Collections.Generic;

namespace Client.App
{
    public static class GlobalData
    {
        static GlobalData()
        {
            CryptoData = new CryptoData();
        }
        public static CryptoData CryptoData { get; }
        public static Communicator Communicator { get; } = new Communicator();
        public static string Login { get; set; }
        public static Dictionary<string, (byte[] pubKey, byte[] IV)> Users { get; set; } = new Dictionary<string, (byte[] pubKey, byte[] IV)>();
    }
}
