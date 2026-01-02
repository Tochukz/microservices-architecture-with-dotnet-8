using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{

    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginReqDto loginReqDto = new();
            return View(loginReqDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginReqDto loginDto)
        {
            ResponseDto response = await _authService.LoginAsync(loginDto);
            if (response != null && response.IsSuccess)
            {
                LoginResDto loginResDto = JsonConvert.DeserializeObject<LoginResDto>(Convert.ToString(response.Result));
                _tokenProvider.SetToken(loginResDto.Token);
                return RedirectToAction("Index", "Home");  
            }
            else
            {
                ModelState.AddModelError("CustomError", response.Message);
                return View(loginDto);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            List<SelectListItem> roles = new()
            {
                new SelectListItem{ Text=StyleDetails.RoleAdmin, Value=StyleDetails.RoleAdmin},
                new SelectListItem{ Text=StyleDetails.RoleCustomer, Value=StyleDetails.RoleCustomer}
            };
            ViewBag.RoleList = roles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationReqDto regDto)
        {
            ResponseDto response = await _authService.RegisterAsync(regDto);
            ResponseDto assignedRole;
            if (response != null && response.IsSuccess)
            {
                if(string.IsNullOrEmpty(regDto.Role))
                {
                    regDto.Role = StyleDetails.RoleCustomer;
                }

                // Assign Role
                assignedRole = await _authService.AssignRoleAsync(regDto);

                if (assignedRole != null && assignedRole.IsSuccess)
                {
                    TempData["success"]= "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
                    
            }

            List<SelectListItem> roles = new()
            {
                new SelectListItem{ Text=StyleDetails.RoleAdmin, Value=StyleDetails.RoleAdmin},
                new SelectListItem{ Text=StyleDetails.RoleCustomer, Value=StyleDetails.RoleCustomer}
            };
            ViewBag.RoleList = roles;
            return View(regDto);
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
