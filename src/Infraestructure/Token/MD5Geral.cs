using System.Security.Cryptography;
using System.Text;

namespace Infraestructure.Token
{
    public static class MD5Geral
    {
        public static string CriptogragarMD5(string password)
        {
            using (var algorithm = MD5.Create())
            {
                var hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

    }
}
