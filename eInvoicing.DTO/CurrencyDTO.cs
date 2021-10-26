using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class CurrencyDTO
    {
        public string name { get; set; }
        public string symbol_native { get; set; }
        public string decimal_digits { get; set; }
        public string code { get; set; }
        public string name_plural { get; set; }
        public string rounding { get; set; }
    }
}