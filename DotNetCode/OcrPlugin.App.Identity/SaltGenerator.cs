using System.Security.Cryptography;

namespace OcrPlugin.App.Identity
{
    internal static class SaltGenerator
    {
        internal static byte[] Get(int maximumSaltLength = 32)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }
    }
}