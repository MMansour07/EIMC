﻿using eInvoicing.DomainEntities.Entities;
using eInvoicing.Persistence;
using eInvoicing.Repository.Contract;
using eInvoicing.Repository.Implementation.Base;
using System;
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
    }
}
