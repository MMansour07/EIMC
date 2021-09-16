using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DomainEntities.Base;

namespace eInvoicing.DomainEntities.Entities
{
    public class Error: BaseEntity
    {
        [Key]
        public new int Id { get; set; }
        public string message { get; set; }
        public string target { get; set; }
        public string propertyPath { get; set; }
        public string code { get; set; }
        public string DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }
    }
}
