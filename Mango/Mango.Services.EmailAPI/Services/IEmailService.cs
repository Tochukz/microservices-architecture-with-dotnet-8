using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.EmailAPI.Models.Dto;

namespace Mango.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task EmailRegistrationAndLog(RegistrationReqDto registrationReqDto);
    }
}
