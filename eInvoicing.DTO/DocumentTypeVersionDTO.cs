using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class DocumentTypeVersionDTO: BaseDTO
    {
        public int id { get; set; }
        public string typeName { get; set; }
        public string name { get; set; }
        public string versionNumber { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string activeFrom { get; set; }
        public string activeTo { get; set; }
        public string jsonschema { get; set; }
        public string xmlschema { get; set; }
    }
}
