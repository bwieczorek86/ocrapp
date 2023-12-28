using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace OcrPlugin.App.Identity
{
    public interface IPasswordHasher
    {
        public string Hash(string password, byte[] salt)
        {
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10,
                    numBytesRequested: 256 / 8));
        }
    }
}