using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefe.Core
{
    public class PagedProcessResult : ProcessResult
    {
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
    }
}
