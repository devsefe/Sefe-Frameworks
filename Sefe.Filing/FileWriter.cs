using Sefe.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefe.Filing
{
    public class FileWriter
    {
        public ProcessResult WriteFile(string path, string text)
        {
            return this.WriteFile(path, text, false);
        }
        public ProcessResult WriteFile(string path, string text, bool append)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                TextWriter tw = new StreamWriter(path, append);
                tw.WriteLine(text);
                tw.Close();
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }
            return result;
        }
        /// <summary>
        /// The given path array is created and the "text" value is written to the file.
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="text">text</param>
        /// <param name="fileName">file name</param>
        /// <param name="appendToText">Is the new text added to the current file?</param>
        /// <returns></returns>
        public ProcessResult WriteFile(string path, string text, string fileName, bool appendToText)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(fileName))
                {
                    result = this.WriteFile(Path.Combine(path, fileName), text, appendToText);
                    if (!result.IsSuccess())
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }
            return result;
        }
        /// <summary>
        /// Create a folder path according to the current directory.
        /// Ex full path: C:\ProgramFiles\2015\05\28\13\30
        /// Ex only date path: C:\ProgramFiles\2015\05\28
        /// </summary>
        /// <param name="basePath">The home directory where folders will be created</param>
        /// <param name="onlyDate">If this value is set to "true", only the date is set.</param>
        /// <returns></returns>
        public string GenerateDateTimeDirectoryPath(string basePath, bool onlyDate)
        {
            StringBuilder builder = new StringBuilder(basePath);
            builder.Append(@"\");
            builder.Append(DateTime.Now.Year.ToString());
            builder.Append(@"\");
            builder.Append(DateTime.Now.Month.ToString());
            builder.Append(@"\");
            builder.Append(DateTime.Now.Day.ToString());
            if (!onlyDate)
            {
                builder.Append(@"\");
                builder.Append(DateTime.Now.Hour.ToString());
                builder.Append(@"\");
                builder.Append(DateTime.Now.Minute.ToString());
            }
            return builder.ToString();
        }
        /// <summary>
        /// It is created by checking the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ProcessResult CreateDirectory(string path)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                result.SystemError = ex;
                result.ResultType = ProcessResultTypes.SystemError;
                return result;
            }
            return result;
        }
        /// <summary>
        /// Folders are created according to the given directory given in the given directory.
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="createDateTimePath">Do you create folders based on the current date?</param>
        /// <param name="onlyDate">Only use date</param>
        /// <returns></returns>
        public ProcessResult CreateDirectory(string path, bool createDateTimePath, bool onlyDate = true)
        {
            ProcessResult result = new ProcessResult();
            if (createDateTimePath)
            {
                result = this.CreateDirectory(this.GenerateDateTimeDirectoryPath(path, onlyDate));
            }
            else
            {
                result = this.CreateDirectory(path);
            }
            return result;
        }
    }
}
