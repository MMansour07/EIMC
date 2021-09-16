using eInvoicing.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class CodeCategorization: BaseDTO
    {
        public Level level1 { get; set; }
        public Level level2 { get; set; }
        public Level level3 { get; set; }
        public Level level4 { get; set; }
    }
}
