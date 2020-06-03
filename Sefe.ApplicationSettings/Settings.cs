using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sefe.Extensions;
using System.Web;

namespace Sefe.ApplicationSettings
{
    /// <summary>
    /// Gets application settings from config file
    /// Gets query string values
    /// Gets session values
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Getting "string" value from config file
        /// </summary>
        /// <param name="key">Key for searching</param>
        /// <param name="defaultValue">The value to return if the value searched for does not exist.</param>
        /// <returns></returns>
        public static string GetAppSetting(string key, string defaultValue)
        {
            string value = System.Configuration.ConfigurationSettings.AppSettings[key];
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            return value;
        }
        /// <summary>
        /// Gets "int" value from Config file. If the value is not "int", returns "defaultValue"
        /// </summary>
        /// <param name="key">Key for searching</param>
        /// <param name="defaultValue">The value to return if the value searched for does not exist.</param>
        /// <returns></returns>
        public static int GetAppSetting(string key, int defaultValue)
        {
            string value = System.Configuration.ConfigurationSettings.AppSettings[key];
            int? val = value.ToInt(null);
            if (val == null)
            {
                return defaultValue;
            }
            return val.Value;
        }
        /// <summary>
        /// Gets "decimal" value from Config file. If the value is not "decimal", returns "defaultValue"
        /// </summary>
        /// <param name="key">Key for searching</param>
        /// <param name="defaultValue">The value to return if the value searched for does not exist.</param>
        /// <returns></returns>
        public static decimal GetAppSetting(string key, decimal defaultValue)
        {
            string value = System.Configuration.ConfigurationSettings.AppSettings[key];
            decimal? val = value.ToDecimal(null);
            if (val == null)
            {
                return defaultValue;
            }
            return val.Value;
        }
        /// <summary>
        /// Gets "boolean" value from Config file. If the value is not "boolean", returns "defaultValue"
        /// </summary>
        /// <param name="key">Key for searching</param>
        /// <param name="defaultValue">The value to return if the value searched for does not exist.</param>
        /// <returns></returns>
        public static bool GetAppSetting(string key, bool defaultValue)
        {
            string value = System.Configuration.ConfigurationSettings.AppSettings[key];
            bool? val = value.ToBool(null);
            if (val == null)
            {
                return defaultValue;
            }
            return val.Value;
        }

        /// <summary>
        /// Getting "string" value from query string
        /// </summary>
        /// <param name="key">Key for searching</param>
        /// <param name="defaultValue">The value to return if the value searched for does not exist.</param>
        /// <returns></returns>
        public static string GetQueryString(string key, string defaultValue)
        {
            string value = HttpContext.Current.Request[key];
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            return value;
        }
        /// <summary>
        /// Gets "int" value from query string file. If the value is not "int", returns "defaultValue"
        /// </summary>
        /// <param name="key">Key for searching</param>
        /// <param name="defaultValue">The value to return if the value searched for does not exist.</param>
        /// <returns></returns>
        public static int GetQueryString(string key, int defaultValue)
        {
            string value = HttpContext.Current.Request[key];
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            int? val = value.ToInt(null);
            if (!val.HasValue)
            {
                return defaultValue;
            }
            return val.Value;
        }
        /// <summary>
        /// Gets "byte" value from query string file. If the value is not "byte", returns "defaultValue"
        /// </summary>
        /// <param name="key">Key for searching</param>
        /// <param name="defaultValue">The value to return if the value searched for does not exist.</param>
        /// <returns></returns>
        public static byte GetQueryString(string key, byte defaultValue)
        {
            string value = HttpContext.Current.Request[key];
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            byte? val = value.ToByte(null);
            if (!val.HasValue)
            {
                return defaultValue;
            }
            return val.Value;
        }

        /// <summary>
        /// Gets "string" value from session file. If the value is not "string" or null, returns "defaultValue"
        /// </summary>
        /// <param name="key">Key for searching</param>
        /// <param name="defaultValue">The value to return if the value searched for does not exist.</param>
        /// <returns></returns>
        public static string GetSessionValue(string key, string defaultValue)
        {
            object value = HttpContext.Current.Session[key];
            if (value == null)
            {
                return defaultValue;
            }
            return value.ToString();
        }
        /// <summary>
        /// Gets "int" value from session file. If the value is not "int" or null, returns "defaultValue"
        /// </summary>
        /// <param name="key">Key for searching</param>
        /// <param name="defaultValue">The value to return if the value searched for does not exist.</param>
        /// <returns></returns>
        public static int GetSessionValue(string key, int defaultValue)
        {
            object value = HttpContext.Current.Session[key];
            if (value == null)
            {
                return defaultValue;
            }
            int? val = value.ToString().ToInt(null);
            if (!val.HasValue)
            {
                return defaultValue;
            }
            return val.Value;
        }
        /// <summary>
        /// Gets "object" value from session file. If the value is null, returns "defaultValue"
        /// </summary>
        /// <param name="key">Key for searching</param>
        /// <param name="defaultValue">The value to return if the value searched for does not exist.</param>
        /// <returns></returns>
        public static object GetSessionValue(string key, object defaultValue)
        {
            object value = HttpContext.Current.Session[key];
            if (value == null)
            {
                return defaultValue;
            }
            return value;
        }
    }
}