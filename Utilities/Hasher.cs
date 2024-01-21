using PuppyLearn.Services;
using System.Security.Cryptography;
using System.Text;

namespace PuppyLearn.Utilities
{
    public static class Hasher
    {
        private static int _keySize = Int32.Parse(AppServiceHelper.configuration.GetSection("Hasher:KeySize").Value);
        private static int _iteration = Int32.Parse(AppServiceHelper.configuration.GetSection("Hasher:Iteration").Value);

        public static string ComputeHash(string password, byte[] salt)
        {
            var hashedPassword = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
            salt,
                _iteration,
                HashAlgorithmName.SHA256,
                _keySize);
            return Convert.ToBase64String(hashedPassword);
        }

        public static bool VerifyPassword(string password, string hashedPassword, byte[] salt)
        {
            var loginHashedPassword = Hasher.ComputeHash(password, salt);
            return CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(loginHashedPassword), Convert.FromBase64String(hashedPassword));
        }

        public static string GenerateSalt()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(_keySize));
        }
    }
}