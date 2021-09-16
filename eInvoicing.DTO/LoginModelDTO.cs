using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class LoginModelDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}