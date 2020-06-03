using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sefe.Extentions
{
    public static class StringExtentions
    {
        /// <summary>
        /// Clear the HTML tags in the given string variable.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveHtmlTags(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return value;

            return Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
        }

        /// <summary>
        /// If the length of the given text is greater than the maximum length given, it is shortened by the maximum value and puts three dots at the end.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string Crop(this string text, int maxLength)
        {
            string value = text;
            if (string.IsNullOrEmpty(text))
            {
                return value;
            }
            if (text.Length > maxLength)
            {
                int length = maxLength - 3 < 0 ? 0 : maxLength - 3;
                value = string.Format("{0}...", text.Substring(0, length));
                GC.SuppressFinalize(length);
            }
            return value;
        }

        /// <summary>
        /// It checks whether the text being written is in e-mail format or not.
        /// </summary>
        /// <param name="emailText"></param>
        /// <returns></returns>
        public static bool IsEmail(this string emailText)
        {
            if (string.IsNullOrEmpty(emailText))
                return false;
            string pattent = @"^(([^<>()[\]\\.,;:\s@\""]+"
                + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                + @"[a-zA-Z]{2,}))$";
            return Regex.IsMatch(emailText, pattent);
        }
    }
}
