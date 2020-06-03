using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefe.Extensions
{
    /// <summary>
    /// Extension methods for convert operations
    /// </summary>
    public static class Converting
    {
        /// <summary>
        /// Convert to byte
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="defaultValue">The value to return when the dial operation fails.</param>
        /// <returns></returns>
        public static byte? ToByte(this string value, byte? defaultValue)
        {
            byte val;
            if (!byte.TryParse(value, out val))
            {
                return defaultValue;
            }
            return val;
        }
        /// <summary>
        /// Convert to Int32
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="defaultValue">The value to return when the dial operation fails</param>
        /// <returns></returns>
        public static int? ToInt(this string value, int? defaultValue)
        {
            int val;
            if (!int.TryParse(value, out val))
            {
                return defaultValue;
            }
            return val;
        }
        /// <summary>
        /// Convert to decimal
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="defaultValue">The value to return when the dial operation fails</param>
        /// <returns></returns>
        public static decimal? ToDecimal(this string value, decimal? defaultValue)
        {
            decimal val;
            if (!decimal.TryParse(value, out val))
            {
                return defaultValue;
            }
            return val;
        }
        /// <summary>
        /// Convert to double
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="defaultValue">The value to return when the dial operation fails</param>
        /// <returns></returns>
        public static double? ToDouble(this string value, double? defaultValue)
        {
            double val;
            if (!double.TryParse(value, out val))
            {
                return defaultValue;
            }
            return val;
        }
        /// <summary>
        /// Convert to boolean
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="defaultValue">The value to return when the dial operation fails</param>
        /// <returns></returns>
        public static bool? ToBool(this string value, bool? defaultValue)
        {
            bool val;
            if (!bool.TryParse(value, out val))
            {
                return defaultValue;
            }
            return val;
        }
        /// <summary>
        /// Convert to DateTime
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="defaultValue">The value to return when the dial operation fails</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value, DateTime? defaultValue)
        {
            DateTime val;
            if (!DateTime.TryParse(value, out val))
            {
                return defaultValue;
            }
            return val;
        }
    }
}
