using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DatingApp.API.Dtos
{
    public class RegistrationModelDTO
    {
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        //[RegularExpression(@"^([A-Za-z0-9][^'!&\\#*$%^?<>()+=:;`~\[\]{}|/,₹€@ ][a-zA-z0- 9-._][^!&\\#*$%^?<>()+=:;`~\[\]{}|/,₹€@ ]*\@[a-zA-Z0-9][^!&@\\#*$%^?<> ()+=':;~`.\[\]{}|/,₹€ ]*\.[a-zA-Z]{2,6})$", ErrorMessage = "Please enter a valid Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(8)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public string BusinessGroupId { get; set; }
        public string SlashSeparatedRoles { get; set; }
    }
}