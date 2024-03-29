﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class RejectedErrorDTO : BaseDTO
    {
        public string code { get; set; }
        public string message { get; set; }
        public string target { get; set; }
        public string propertyPath { get; set; }
        public List<RejectedErrorDTO> details { get; set; }
    }
}
