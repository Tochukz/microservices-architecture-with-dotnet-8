using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto>? LoginAsync(LoginReqDto loginReqDto);
        Task<ResponseDto>? RegisterAsync(RegistrationReqDto registrationReqDto);
        Task<ResponseDto>? AssignRoleAsync(RegistrationReqDto registrationReqDto);
    }
}
 