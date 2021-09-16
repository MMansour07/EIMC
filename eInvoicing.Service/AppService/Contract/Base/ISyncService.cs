using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface ISyncService
    {
        List<Document> GetDocumentsFromVW_Documents(string _connectionstring, string IssuedDate);
        List<InvoiceLine> GetInvoiceLinesFromVW_InvoiceLines(string _connectionstring, string IssuedDate);
        List<TaxableItem> GetTaxableItemsFromVW_TaxableItems(string _connectionstring, string IssuedDate);
        List<Document> InsertBulkIntoDocumentsTbl(string _connectionstring, string IssuedDate);
        List<InvoiceLine> InsertBulkIntoInvoiceLinesTbl(string _connectionstring, string IssuedDate);
        List<TaxableItem> InsertBulkIntoTaxableItemsTbl(string _connectionstring, string IssuedDate);
        void SyncFromViewToTbl();
    }
}
