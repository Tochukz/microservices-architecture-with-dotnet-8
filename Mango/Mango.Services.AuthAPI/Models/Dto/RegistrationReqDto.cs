namespace Mango.Services.AuthAPI.Models.Dto
{
    public class RegistrationReqDto
    {
        public string Email { set; get; }
        public string Name {  set; get; }
        public string PhoneNumber { set; get; }
        public string Password { set; get; }
        public string? Role { set; get; }
    }
}
