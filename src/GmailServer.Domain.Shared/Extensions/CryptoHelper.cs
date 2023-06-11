using System;
using System.Security.Cryptography;
using System.Text;

namespace GmailServer.Extensions
{
    public static class CryptoHelper
    {
        public static string CreateMD5(this string text)
        {
            using (var md5 = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                return Convert.ToBase64String(md5.ComputeHash(bytes));
            }
        }

        public static string CreateSHA256(this string text)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                return Convert.ToBase64String(sha256.ComputeHash(bytes));
            }
        }

    }
}
