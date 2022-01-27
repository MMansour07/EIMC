using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eInvoicing.DTO
{
    public class EditModelDTO
    {
        [Required(ErrorMessage = "Required Field.")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Required Field.")]
        public string PhoneNumber { get; set; }
        public string Id { get; set; }
        [Required(ErrorMessage = "Required Field.")]

        public string Title { get; set; }
        [Required(ErrorMessage = "Required Field.")]

        public string FirstName { get; set; }
        [Required(ErrorMessage = "Required Field.")]

        public string LastName { get; set; }
        [Required(ErrorMessage = "Required Field.")]

        public List<string> Roles { get; set; }
        public string BusinessGroupId { get; set; }
        public string SlashSeparatedRoles { get; set; }
    }
}
