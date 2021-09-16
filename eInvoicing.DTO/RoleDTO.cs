using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class RoleDTO: BaseDTO
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Required Field.")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Required Field.")]

        public string Description { get; set; }
        [Required(ErrorMessage = "Required Field.")]

        public List<string> Privileges { get; set; }
        [Required(ErrorMessage = "Required Field.")]

        public List<string> Permissions { get; set; }
    }
}
