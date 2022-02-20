using eInvoicing.DTO.Base;
using System.Collections.Generic;
using System.Net;

namespace eInvoicing.DTO
{
    public class SearchPublishedCodesResponseDTO : BaseDTO
    {
        public List<SearchPublishedCodesResultDTO> result { get; set; }
        public MetaDataDTO metadata { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
