using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class EmailContentDTO
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string BusinessGroup { get; set; }
        public string Message { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string Error { get; set; }
        public int SentCount { get; set; }
        public int FailedCount { get; set; }
        public string HTML { get; set; }
    }
}
