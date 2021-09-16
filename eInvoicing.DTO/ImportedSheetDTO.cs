using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class ImportedSheetDTO
    {
        public  int InsertedDocumentsCount { get; set; }
        public int InsertedInvoiceLinesCount { get; set; }
        public int InsertedTaxableItemsCount { get; set; }
        public int UpdatedDocumentsCount { get; set; }
        public int UpdatedInvoiceLinesCount { get; set; }
        public int UpdatedTaxableItemsCount { get; set; }
        public bool IsInserted { get; set; }
        public bool IsUpdated { get; set; }
    }
}
