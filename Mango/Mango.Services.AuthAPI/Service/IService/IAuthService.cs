using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationReqDto registrationReqDto);
        Task<LoginResDto> Login(LoginReqDto loginReqDto);
        Task<bool> AssignRole(string email, string roleName);
    }
}
