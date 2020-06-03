using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefe.Extentions
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// Gets the "Message" value of system errors. If the error's "Inner Exception" property is not null, it is included in the message.
        /// </summary>
        /// <param name="exception">Error</param>
        /// <returns>string message</returns>
        public static string GetExceptionMessage(this Exception exception)
        {
            string message = string.Empty;
            if (exception.InnerException == null)
            {
                message = string.Format("Exception:{0}\r\nStack Trace:{1}"
                    , exception.Message, exception.StackTrace);
            }
            else
            {
                message = string.Format("Exception:{0}\r\nStack Trace:{1}\r\nInnerException:{2}\r\nInnerException Stack Trace:{3}"
                   , exception.Message, exception.StackTrace, exception.InnerException.Message, exception.InnerException.StackTrace);
            }
            return message;
        }
    }
}
