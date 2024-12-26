namespace Anti_RecoilApplicationAPI.Helpers
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            // Example using BCrypt for password hashing
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string passwordHash)
        {
            // Example using BCrypt for password hashing
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
