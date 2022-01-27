using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class DocumentResponse
    {
        public Meta meta { get; set; }
        public PagedList<DocumentVM> data { get; set; }
    }
    public class SubmittedDocumentResponse
    {
        public Meta meta { get; set; }
        public PagedList<SubmittedDocumentsDTO> data { get; set; }
    }
    public class GoodsModelVM
    {
        public Meta meta { get; set; }
        public PagedList<GoodsModel> data { get; set; }
    }
    public class GetMonthlyBestSellerResponse
    {
        public Meta meta { get; set; }
        public List<GoodsModel> data { get; set; }
    }

    public class InvalidDocumentResponse
    {
        public Meta meta { get; set; }
        public PagedList<InvalidDocumentsReasonsDTO> data { get; set; }
    }
}
