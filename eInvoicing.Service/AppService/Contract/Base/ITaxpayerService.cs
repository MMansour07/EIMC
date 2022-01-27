using eInvoicing.DomainEntities.Entities;
using eInvoicing.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.Service.AppService.Contract.Base
{
    public interface ITaxpayerService
    {
        void add(TaxpayerDTO obj);
        void updateTaxPayer(TaxpayerDTO obj);
        TaxpayerDTO getTaxpayerDetails(string BusinessGroupId);
        TaxpayerDTO getTaxpayerDetailsByBusinessGroupName(string BusinessGroup);
        string TokenByBusinessGroup(string BusinessGroup);
        string TokenByRegistrationNumber(string BusinessGroup);
        string GetClientIdByBusinessGroup(string BusinessGroup);
        string GetClientIdByRegistrationNumber(string BusinessGroupId);
        bool CheckIsDBSync(string BusinessGroup);
    }
}
