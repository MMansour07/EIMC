using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class SearchEGSCodeResponseDTO : BaseDTO
    {
        public List<SearchEGSCodeResultDTO> result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public MetaDataDTO metadata { get; set; }
    }
}
