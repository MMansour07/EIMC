using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class UNITVALUESDTO: BaseDTO
    {
        public string currencySold { get; set; }
        public decimal amountEGP { get; set; }
        public decimal amountSold { get; set; }
        public decimal currencyExchangeRate { get; set; }
    }
}
