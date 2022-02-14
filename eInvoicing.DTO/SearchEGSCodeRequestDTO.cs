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
        public bool active { get; set; }
        public string status { get; set; }
        public string ItemCode { get; set; }
        public string CodeName { get; set; }
        public string ParentLevelName { get; set; }
        public string ParentItemCode { get; set; }
        public string CodeDescription { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public string orderDirections { get; set; }
    }
}