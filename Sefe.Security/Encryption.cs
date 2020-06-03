using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sefe.Security
{
    public static class Encryption
    {
        public static string HashString(string text)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] data = Encoding.UTF8.GetBytes(text);
            data = md5.ComputeHash(data);

            StringBuilder builder = new StringBuilder();
            foreach (byte item in data)
            {
                builder.Append(item.ToString("x2"));
            }
            return builder.ToString();
        }
        public static string GenerateRandomString(int length)
        {
            char[] charArr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
            string randomString = string.Empty;
            Random objRandom = new Random();
            for (int i = 0; i < length; i++)
            {
                //Don't Allow Repetation of Characters
                int x = objRandom.Next(1, charArr.Length);
                if (!randomString.Contains(charArr.GetValue(x).ToString()))
                    randomString += charArr.GetValue(x);
                else
                    i--;
            }
            return randomString;
        }
        public static string SHA256Hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
