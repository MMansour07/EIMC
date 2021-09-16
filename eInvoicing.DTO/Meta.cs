using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class Meta
    {
        public int page { get; set; }
        public int pages { get; set; }
        public int perpage { get; set; }
        public int total { get; set; }
        public int totalFiltered { get; set; }
    }
    public class DatatableInputParam
    {
        public int start { get; set; }
        public int length { get; set; }
        public string dir { get; set; }
        public string fromDate { get; set; }
        public int specificDate { get; set; }
        public string toDate { get; set; }
    }
}
