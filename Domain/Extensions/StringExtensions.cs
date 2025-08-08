using System.Security.Cryptography;
using System.Text;

namespace Domain.Extensions
{
    public static class StringExtensions
    {
        public static string CriptografarSenha(this string password)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sBuilder.Append(hashBytes[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
    }
}