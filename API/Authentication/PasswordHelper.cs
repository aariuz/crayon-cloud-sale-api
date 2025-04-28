using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace API.Authentication
{
    public static class PasswordHelper
    {
        private static int DegreeOfParallelism = 8;
        private static int MemorySize = 65536;
        private static int Iterations = 4;

        public static (string HashBase64, string SaltBase64) HashPassword(string password)
        {
            // 16-byte random salt
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            using (var argon2 = new Argon2id(passwordBytes))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = DegreeOfParallelism;
                argon2.MemorySize = MemorySize;
                argon2.Iterations = Iterations;

                byte[] hash = argon2.GetBytes(32);

                return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
            }
        }

        public static bool VerifyPassword(string password, string hashBase64, string saltBase64)
        {
            byte[] salt = Convert.FromBase64String(saltBase64);
            byte[] expectedHash = Convert.FromBase64String(hashBase64);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            using (var argon2 = new Argon2id(passwordBytes))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = DegreeOfParallelism;
                argon2.MemorySize = MemorySize;
                argon2.Iterations = Iterations;

                byte[] computedHash = argon2.GetBytes(32);

                return CryptographicOperations.FixedTimeEquals(expectedHash, computedHash);
            }
        }
    }
}
