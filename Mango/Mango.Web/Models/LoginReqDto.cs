using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class LoginReqDto
    {
        [Required]
        public string UserName { set; get; }
        [Required]
        public string Password { set; get; }
    }
}
