using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sefe.Core.Json
{
    /// <summary>
    /// Process result for json operations
    /// </summary>
    [Serializable]
    [DataContract]
    public class JProcessResult
    {
        [DataMember]
        /// <summary>
        /// error message
        /// </summary>
        public string message { get; set; }
        
        [DataMember]
        /// <summary>
        /// success
        /// error
        /// </summary>
        public string status { get; set; }

        [DataMember]
        /// <summary>
        /// json data
        /// </summary>
        public object data { get; set; }

        public JProcessResult()
        {
        }

        public JProcessResult(string message, string status, object data)
        {
            this.data = data;
            this.message = message;
            this.status = status;
        }
    }
}
