using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class GetPackageRequestsRsDTO: BaseDTO
    {
        public List<GetPackageRequestsResultDTO> result { get; set; }
        public MetaDataDTO metadata { get; set; }
        public ErrorDTO error { get; set; }

    }
}
