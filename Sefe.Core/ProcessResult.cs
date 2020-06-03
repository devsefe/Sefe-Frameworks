using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sefe.Extentions;

namespace Sefe.Core
{
    /// <summary>
    /// Use for get any process result. You can use for all logical operations.
    /// </summary>
    public class ProcessResult
    {
        /// <summary>
        /// Error code that selected by developer
        /// </summary>
        public ProcessResultErrorCodes ErrorCode { get; set; }
        /// <summary>
        /// System error (try catch exception)
        /// </summary>
        public Exception SystemError { get; set; }
        /// <summary>
        /// Error message that written by developer
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Return value
        /// </summary>
        public object ReturnObject { get; set; }
        /// <summary>
        /// Error list that created by developer
        /// </summary>
        public List<string> ErrorList { get; set; }
        /// <summary>
        /// Process result type
        /// </summary>
        public ProcessResultTypes ResultType { get; set; }
        /// <summary>
        /// Anything what you want to return
        /// </summary>
        public object Pocket { get; set; }

        public ProcessResult()
        {
            this.ResultType = ProcessResultTypes.Success;
            this.ErrorList = new List<string>();
        }

        public bool IsSuccess()
        {
            return this.ResultType == ProcessResultTypes.Success;
        }
        /// <summary>
        /// Gets the "Message" value of system errors.
        /// </summary>
        /// <returns></returns>
        public string ReadSystemError()
        {
            if (this.ResultType != ProcessResultTypes.SystemError)
            {
                return string.Empty;
            }
            return this.SystemError.GetExceptionMessage();
        }
    }
}