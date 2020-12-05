using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Server.Logic
{
    public class CryptoData
    {
        public byte[] serverPublicKey { get; }
        private byte[] _privateKey;
        private ECDiffieHellmanCng _server;
        public byte[] IV;
        public CryptoData()
        {
            _server = new ECDiffieHellmanCng();
            _server.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            _server.HashAlgorithm = CngAlgorithm.Sha256;
            serverPublicKey = _server.PublicKey.ToByteArray();
        }
        public void GenerateKey(byte[] sessionPublicKey, byte[] iv)
        {
            IV = iv;
            _privateKey = _server.DeriveKeyMaterial(CngKey.Import(sessionPublicKey, CngKeyBlobFormat.EccPublicBlob));
        }
        public string Decrypt(byte[] encriptedMessage)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.Key = _privateKey;
                aes.IV = IV;
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
