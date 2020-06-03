using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Sefe.Extentions
{
    public static class FileExtensions
    {
        public static byte[] GetFileBytes(this HttpPostedFileBase selectedFile)
        {
            byte[] fileBytes = null;
            using (var binaryReader = new System.IO.BinaryReader(selectedFile.InputStream))
            {
                fileBytes = binaryReader.ReadBytes(selectedFile.ContentLength);
            }
            return fileBytes;
        }
    }
}
