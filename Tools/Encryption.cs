using System.Security.Cryptography;
using System.Text;

namespace Tools
{
    static public class Encryption
    {
        public static string GetHash(string s)
        {
            HashAlgorithm algorithm = SHA256.Create();
            byte[] bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(s));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("X2"));
            }
            return builder.ToString();
        }
    }
}
