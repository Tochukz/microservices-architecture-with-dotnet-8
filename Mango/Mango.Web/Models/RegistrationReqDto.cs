using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class RegistrationReqDto
    {
        [Required]
        public string Email { set; get; }
        [Required]
        public string Name {  set; get; }
        [Required]
        public string PhoneNumber { set; get; }
        [Required]
        public string Password { set; get; }
        public string? Role { set; get; }
    }
}
