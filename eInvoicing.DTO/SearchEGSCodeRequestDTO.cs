using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class SearchEGSCodeRequestDTO: BaseDTO
    {
        public Boolean active { get; set; }
        public string status { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public string orderDirections { get; set; }
    }
}
