using System.Security.Cryptography;
using System.Text;

namespace Cliente.API.Helpers
{
    public static class StringHelper
    {
        public static string EncryptSha256(this string texto)
        {
            byte[] byteArray = SHA256.HashData(Encoding.UTF8.GetBytes(texto));
            return Convert.ToHexString(byteArray);
        }
    }
}
