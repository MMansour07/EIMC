using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class UNITVALUESVM: BaseDTO
    {
        public string currencySold { get; set; }
        public string amountEGP { get; set; }
        public string amountSold { get; set; }
        public string currencyExchangeRate { get; set; }
    }
}
