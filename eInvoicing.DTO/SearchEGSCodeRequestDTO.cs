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
        public string active { get; set; }
        public string status { get; set; }
        public string codeType { get; set; }
        public string itemCode { get; set; }
        public string codeName { get; set; }
        public string parentLevelName { get; set; }
        public string parentItemCode { get; set; }
        public string codeDescription { get; set; }
        public string activeFrom { get; set; }
        public string activeTo { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public string orderDirections { get; set; }
    }
}