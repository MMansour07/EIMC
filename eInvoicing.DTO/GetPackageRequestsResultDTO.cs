using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class GetPackageRequestsResultDTO: BaseDTO
    {
        public string packageId { get; set; }
        public DateTime submissionDate { get; set; }
        public int status { get; set; }
        public int type { get; set; }
        public int format { get; set; }
        public int requesterTypeId { get; set; }
        public string requestorTaxpayerRIN { get; set; }
        public DateTime deletionDate { get; set; }
        public bool isExpired { get; set; }
        public QueryParameters queryParameters { get; set; }
    }
}
