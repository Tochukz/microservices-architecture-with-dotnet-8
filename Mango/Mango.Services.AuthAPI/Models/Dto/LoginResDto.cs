namespace Mango.Services.AuthAPI.Models.Dto
{
    public class LoginResDto
    {
        public UserDto User { set; get; }
        public string Token { set; get; }
    }
}
