using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class SubmitInput
    {
        public List<DocumentVM> documents { get; set; }
        public string token { get; set; }
        public string SRN { get; set; }
        public string pin { get; set; }
        public string url { get; set; }
        public string docuemntTypeVersion { get; set; }

    }
}
