using Newtonsoft.Json;
using Sefe.Core;
using Sefe.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Sefe.Serialization
{
    public class Serializing
    {
        public ProcessResult Serialize(object model)
        {
            ProcessResult result = new ProcessResult();

            try
            {
                result.ReturnObject = JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                result.ResultType = ProcessResultTypes.LogicError;
                result.ErrorMessage = ex.GetExceptionMessage();
                return result;
            }

            return result;
        }

        public ProcessResult DeSerialize(string json, Type type)
        {
            ProcessResult result = new ProcessResult();

            try
            {
                result.ReturnObject = JsonConvert.DeserializeObject(json, type);
            }
            catch (Exception ex)
            {
                result.ResultType = ProcessResultTypes.LogicError;
                result.ErrorMessage = ex.GetExceptionMessage();
                return result;
            }

            return result;
        }

        /// <summary>
        /// It serializes the given model and cooks cookies json...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cookieName">cookie name</param>
        /// <param name="newModel">model</param>
        /// <param name="expireMinute">cookie nin cancellation time (min)</param>
        public ProcessResult AddModelToCookie<T>(string cookieName, T newModel, byte expireMinute)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                var serializer = new JavaScriptSerializer();
                HttpCookie cookie;
                T savedModel;
                if (HttpContext.Current.Request.Cookies[cookieName] == null)
                {
                    cookie = new HttpCookie(cookieName);
                    savedModel = newModel;
                }
                else
                {
                    cookie = HttpContext.Current.Request.Cookies[cookieName];
                    savedModel = serializer.Deserialize<T>(cookie.Value);
                    Type type = typeof(T);
                    foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                    {
                        object currentValue = type.GetProperty(pi.Name).GetValue(savedModel, null);
                        object newValue = type.GetProperty(pi.Name).GetValue(newModel, null);

                        if (currentValue != newValue && (currentValue == null || !currentValue.Equals(newValue)))
                        {
                            type.GetProperty(pi.Name).SetValue(savedModel, newValue, null);
                        }
                    }
                }
                cookie.Value = serializer.Serialize(savedModel);
                cookie.Expires = DateTime.Now.AddMinutes(expireMinute);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception ex)
            {
                result.ResultType = ProcessResultTypes.SystemError;
                result.SystemError = ex;
                return result;
            }
            return result;
        }

        /// <summary>
        /// In the given model type, it reads the json data cookie.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="model">Model</param>
        /// <returns></returns>
        public ProcessResult ReadModelFromCookie<T>(string cookieName, T model)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
                if (cookie == null)
                {
                    result.ReturnObject = null;
                    return result;
                }
                result.ReturnObject = new JavaScriptSerializer().Deserialize<T>(cookie.Value);
            }
            catch (Exception ex)
            {
                result.ResultType = ProcessResultTypes.SystemError;
                result.SystemError = ex;
                return result;
            }
            return result;
        }
    }
}
