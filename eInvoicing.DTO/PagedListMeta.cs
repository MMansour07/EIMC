using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class PagedListMeta
    {
        public int? page { get; set; }
        public int perpage { get; set; }
        public int pages { get; set; }
        public int total { get; set; }
        public string sort { get; set; }
        public string field { get; set; }
    }
}
