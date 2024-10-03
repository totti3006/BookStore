using System.Security.Cryptography;
using System.Text;

namespace BookStore.Application.Extensions
{
    public static class StringExtension
    {
        public static string HashWithSHA256(this string rawPassword, string? salt)
        {
            using SHA256 sha256 = SHA256.Create();

            byte[] bytes = Encoding.UTF8.GetBytes($"{rawPassword}{salt}");

            byte[] hash = sha256.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
