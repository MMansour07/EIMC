using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class PaginationParams
    {
        public pagination pagination { get; set; }
        public query query { get; set; }
        public Sort sort { get; set; }
    }
    public class pagination
    {
        //private const int MaxPageSize = 100;
        public string field { get; set; }
        public int page { get; set; }
        public int pages { get; set; }
        //private int pageSize { get; set; } = 10;

        public int perpage { get; set; }
    //    {
    //        get
    //        {
    //            return pageSize;
    //        }
    //set
    //        {
    //            pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    //        }
    //    }
        public string sort { get; set; }
        public int total { get; set; }
    }
    public class query
    {
    }
    public class Sort
    {
        public string sort { get; set; }
        public string field { get; set; }
    }
}

