using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Client.App
{
    public class CryptoData
    {
        public byte[] PublicKey { get; set; }
        private static byte[] _privateKey;
        private ECDiffieHellmanCng _server;
        private static AesCryptoServiceProvider _aes;
        public byte[] IV { get; set; }
        public CryptoData()
        {
            _server = new ECDiffieHellmanCng();
            _server.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            _server.HashAlgorithm = CngAlgorithm.Sha256;
            PublicKey = _server.PublicKey.ToByteArray();
            _aes = new AesCryptoServiceProvider();
            IV = _aes.IV;
        }
        public  byte[] Encrypt(string secretMessage)
        {
            _aes.Key = _privateKey;
            var bytes = Encoding.UTF8.GetBytes(secretMessage);
            using (MemoryStream memStream = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(memStream, _aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                }
                byte[] encryptedMessage = memStream.ToArray();
                return encryptedMessage;
            }
        }
        public void GenerateKey(byte[] serverPublicKey)
        {
            _privateKey = _server.DeriveKeyMaterial(CngKey.Import(serverPublicKey, CngKeyBlobFormat.EccPublicBlob));
        }
        public string Decrypt(byte[] encriptedMessage, byte[] Iv)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.Key = _privateKey;
                aes.IV = Iv;
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (var decryptor = aes.CreateDecryptor())
                    {
                        using (CryptoStream cs = new CryptoStream(memStream, decryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(encriptedMessage, 0, encriptedMessage.Length);
                        }
                    }
                    string message = Encoding.UTF8.GetString(memStream.ToArray());
                    return message;
                }
            }
        }
    }
}
