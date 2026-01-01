using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response; 

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _response = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationReqDto registrationReqDto)
        {
            string errorMessage = await _authService.Register(registrationReqDto);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReqDto loginReqDto)
        {
            LoginResDto loginResDto = await _authService.Login(loginReqDto);
            if (loginResDto.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password in incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResDto;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationReqDto registrationReqDto)
        {
            bool successfull = await _authService.AssignRole(registrationReqDto.Email, registrationReqDto.Role?.ToUpper() ?? "");
            if (!successfull)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encounterd";
                return BadRequest(_response);
            }
            _response.Result = successfull;
            return Ok(_response);
        }
    }
}
