using eInvoicing.DomainEntities.Entities;
using eInvoicing.Persistence;
using eInvoicing.Repository.Contract;
using eInvoicing.Repository.Implementation.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eInvoicing.Repository.Implementation
{
    public class DocumentRepository: Repository<Document>, IDocumentRepository
    {
        public DocumentRepository(ApplicationContext context) : base(context)
        {

        }
        public Document GetDocumentByuuid(string uuid)
        {
            try
            {
                return DbSet.AsNoTracking().FirstOrDefault(x => x.uuid == uuid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateBulkDocumentsByIds(List<string> Documentsids)
        {
            try
            {
                var docs = Context.Documents.Where(d => Documentsids.Contains(d.Id)).ToList();
                docs.ForEach(a =>
                {
                    a.Status = "New";
                    a.uuid = null;
                    a.submissionId = null;
                    a.longId = null;
                });
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
        }

        public bool UpdateBulkDocumentsByStatus()
        {
            try
            {
                var docs = Context.Documents.Where(d => (d.Status.ToLower() == "invalid" || d.Status.ToLower() == "failed")).ToList();
                docs.ForEach(a =>
                {
                    a.Status = "New";
                    a.uuid = null;
                    a.submissionId = null;
                    a.longId = null;
                });
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
        }

    }
}
