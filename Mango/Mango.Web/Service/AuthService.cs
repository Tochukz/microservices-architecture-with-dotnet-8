using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto>? AssignRoleAsync(RegistrationReqDto registrationReqDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StyleDetails.ApiType.POST,
                Data = registrationReqDto,
                Url = StyleDetails.AuthAPIBase + "/api/auth/AssignRole",
            });
        }

        public async Task<ResponseDto>? LoginAsync(LoginReqDto loginReqDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StyleDetails.ApiType.POST,
                Data = loginReqDto,
                Url = StyleDetails.AuthAPIBase + "/api/auth/login",
            }, withBearer: false);
        }

        public async Task<ResponseDto>? RegisterAsync(RegistrationReqDto registrationReqDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StyleDetails.ApiType.POST,
                Data = registrationReqDto,
                Url = StyleDetails.AuthAPIBase + "/api/auth/register",
            }, withBearer: false);
        }
    } 
}
