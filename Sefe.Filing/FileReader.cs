using Sefe.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefe.Filing
{
    /// <summary>
    /// File operations
    /// </summary>
    public class FileReader
    {
        public ProcessResult ReadFile(string path)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                TextReader reader = new StreamReader(path);
                result.ReturnObject = reader.ReadLine();
                reader.Close();
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }
            return result;
        }
        public ProcessResult ReadFileBytes(string path)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                result.ReturnObject = File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }
            return result;
        }

    }
}
