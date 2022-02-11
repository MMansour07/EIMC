using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class OnlineDateDTO : BaseDTO
    {
        public string currentDateTime { get; set; }
        public string datetime { get; set; }
        public string abbreviation { get; set; }
        public string day_of_week { get; set; }
        public string day_of_year { get; set; }
        public string utc_datetime { get; set; }
        public string utc_offset { get; set; }
    }
}
