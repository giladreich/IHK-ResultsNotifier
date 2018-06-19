using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IHK.ResultsNotifier.Utils
{
    public static class Encryption
    {
        private const int KEY_SIZE         = 256;
        private const int ITERATIONS_COUNT = 1000;

        public static string Encrypt(string plainText, string key)
        {
            byte[] saltStringBytes = Generate256BitsOfRandomEntropy();
            byte[] ivStringBytes = Generate256BitsOfRandomEntropy();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(key, saltStringBytes, ITERATIONS_COUNT);
            byte[] keyBytes = password.GetBytes(KEY_SIZE / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged
            {
                BlockSize = 256,
                Mode      = CipherMode.CBC,
                Padding   = PaddingMode.PKCS7
            };
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] cipherTextBytes = saltStringBytes;
            cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
            cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();

            memoryStream.Close(); cryptoStream.Close();
            cryptoStream.Dispose(); memoryStream.Dispose(); encryptor.Dispose();
            symmetricKey.Dispose(); password.Dispose();

            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string encyrptedText, string key)
        {
            byte[] cipherTextBytesWithSaltAndIv = Convert.FromBase64String(encyrptedText);
            byte[] saltStringBytes = cipherTextBytesWithSaltAndIv.Take(KEY_SIZE / 8).ToArray();
            byte[] ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(KEY_SIZE / 8)
                .Take(KEY_SIZE / 8).ToArray();
            byte[] cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip(KEY_SIZE / 8 * 2)
                .Take(cipherTextBytesWithSaltAndIv.Length - KEY_SIZE / 8 * 2).ToArray();

            Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(key, saltStringBytes, ITERATIONS_COUNT);
            byte[] keyBytes = password.GetBytes(KEY_SIZE / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged
            {
                BlockSize = 256,
                Mode      = CipherMode.CBC,
                Padding   = PaddingMode.PKCS7
            };

            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            memoryStream.Close(); cryptoStream.Close();
            password.Dispose(); symmetricKey.Dispose(); decryptor.Dispose();
            memoryStream.Dispose(); cryptoStream.Dispose();

            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            byte[] randomBytes = new byte[32];
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            rngCsp.GetBytes(randomBytes);
            rngCsp.Dispose();

            return randomBytes;
        }

        public static string GenerateKey(int iterationCount)
        {
            string tmpKey = String.Empty;
            string results = String.Empty;

            for (int i = iterationCount - 1; i >= 0; i--)
                tmpKey += Path.GetRandomFileName();

            Random rnd = new Random(DateTime.Now.Millisecond);

            return tmpKey.Aggregate(results, (current, t) =>
                current + (rnd.Next(1, 10) % 2 == 0 ? Char.ToUpper(t) : t));
        }
    }
}
