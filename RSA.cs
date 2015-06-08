using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace AccessInvestigation
{
    class RSA
    {
        public static Tuple<string, string> CreateKeyPair()
        {
            CspParameters cspParams = new CspParameters { ProviderType = 1 };

            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(2024, cspParams);

            string publicKey = Convert.ToBase64String(rsaProvider.ExportCspBlob(false));
            string privateKey = Convert.ToBase64String(rsaProvider.ExportCspBlob(true));

            return new Tuple<string, string>(privateKey, publicKey);
        }
        public static string Decrypt(string privateKey, byte[] encryptedBytes)
        {
            CspParameters cspParams = new CspParameters { ProviderType = 1 };
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);

            rsaProvider.ImportCspBlob(Convert.FromBase64String(privateKey));

            byte[] plainBytes = rsaProvider.Decrypt(encryptedBytes, false);

            string plainText = Encoding.UTF8.GetString(plainBytes, 0, plainBytes.Length);

            return plainText;
        }
        public static byte[] Encrypt(string publicKey, string data)
        {
            CspParameters cspParams = new CspParameters { ProviderType = 1 };
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);

            rsaProvider.ImportCspBlob(Convert.FromBase64String(publicKey));

            byte[] plainBytes = Encoding.UTF8.GetBytes(data);
            byte[] encryptedBytes = rsaProvider.Encrypt(plainBytes, false);

            return encryptedBytes;
        }
    }
}
