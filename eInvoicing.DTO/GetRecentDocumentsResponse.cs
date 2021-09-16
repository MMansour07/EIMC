using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class GetRecentDocumentsResponse
    {
        
        public List<RecentDocumentDTO> result { get; set; }
        public MetaDataDTO metadata { get; set; }
    }
}
