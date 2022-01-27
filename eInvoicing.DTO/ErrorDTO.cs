using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class ErrorDTO
    {
        public string errorCode { get; set; }
        public string message { get; set; }
        public string propertyName { get; set; }
        public string error { get; set; }
        public string errorAr { get; set; }
        public string propertyPath { get; set; }
        public List<ErrorDTO> innerError { get; set; }
    }
}