using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.IO;

namespace AccessInvestigation
{
    class AES
    {
        public static Tuple<string, string> GenerateSecretKey()
        {
            int Rfc2898KeygenIterations = 100;
            int AesKeySizeInBits = 128;
            String Password = "VerySecret!";
            byte[] Salt = new byte[16];
            System.Random rnd = new System.Random();
            rnd.NextBytes(Salt);
            Aes aes = new AesManaged();
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = AesKeySizeInBits;
            int KeyStrengthInBytes = aes.KeySize / 8;
            System.Security.Cryptography.Rfc2898DeriveBytes rfc2898 = new System.Security.Cryptography.Rfc2898DeriveBytes(Password, Salt, Rfc2898KeygenIterations);
            aes.Key = rfc2898.GetBytes(KeyStrengthInBytes);
            aes.IV = rfc2898.GetBytes(KeyStrengthInBytes);
            //ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var str = System.Text.Encoding.Default.GetString(aes.Key);
            var str2 = System.Text.Encoding.Default.GetString(aes.IV);

            return new Tuple<string, string>(str, str2);
        }
        public static byte[] AuthEncrypt(string text, byte[] key, byte[] IV)
        {
            Aes aes = new AesManaged();
            aes.Key = key;
            aes.IV = IV;
            // Convert string to byte array
            byte[] src = Encoding.Unicode.GetBytes(text);

            // encryption
            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);
                return dest;
            }
        }

        public static string AuthDecrypt(byte[] text, byte[] key, byte[] IV)
        {
            Aes aes = new AesManaged();
            aes.Key = key;
            aes.IV = IV;
            // encryption
            using (ICryptoTransform decrypt = aes.CreateDecryptor())
            {
                byte[] dest = decrypt.TransformFinalBlock(text, 0, text.Length);
                return Encoding.Unicode.GetString(dest);
            }
        }
        public static void EncryptFile(string inputFile, string outputFile, byte[] key, byte[] IV)
        {
            
            try
            {
                Aes aes = new AesManaged();
                aes.Key = key;
                aes.IV = IV;

                FileStream fsCrypt = new FileStream(outputFile, FileMode.Create);

                ICryptoTransform encryptor = aes.CreateEncryptor(key, IV);

                CryptoStream cs = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                {
                    cs.WriteByte((byte)data);
                }
                cs.Close();
                fsIn.Close();
                fsCrypt.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public static void DecryptFile(string inputFile, string outputFile, byte[] key, byte[] IV)
        {
            try
            {
                using (Aes aes = new AesManaged())
                {
                    aes.Key = key;
                    aes.IV = IV;
                    /* This is for demostrating purposes only.
                     * Ideally you will want the IV key to be different from your key and you should always generate a new one for each encryption in other to achieve maximum security*/

                    using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
                    {
                        using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                        {
                            using (ICryptoTransform decryptor = aes.CreateDecryptor(key, IV))
                            {
                                using (CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read))
                                {
                                    int data;
                                    while ((data = cs.ReadByte()) != -1)
                                    {
                                        fsOut.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}