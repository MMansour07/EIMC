using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class SearchPublishedCodesResponseDTO : BaseDTO
    {
        public List<SearchPublishedCodesResultDTO> result { get; set; }
        public MetaDataDTO metadata { get; set; }
    }
}
