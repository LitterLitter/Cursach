
namespace Server.Logic
{
    public static class GlobalData
    {
        static GlobalData()
        {
            CryptoData = new CryptoData();
        }
        public static CryptoData CryptoData { get; }
    }
}
