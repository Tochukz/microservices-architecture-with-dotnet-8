using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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
        public async Task<IActionResult> Login(LoginReqDto loginReqDto)
        {
            ResponseDto loginApiResp = await _authService.LoginAsync(loginReqDto);
            if (loginApiResp != null && loginApiResp.IsSuccess)
            {
                LoginResDto loginResDto = JsonConvert.DeserializeObject<LoginResDto>(Convert.ToString(loginApiResp.Result));
                await SignInUser(loginResDto);
                _tokenProvider.SetToken(loginResDto.Token);
                return RedirectToAction("Index", "Home");  
            }
            else
            {
                TempData["error"] = loginApiResp.Message;
                return View(loginReqDto);
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
            else
            {
                TempData["error"] = response.Message;
            }

            List<SelectListItem> roles = new()
            {
                new SelectListItem{ Text=StyleDetails.RoleAdmin, Value=StyleDetails.RoleAdmin},
                new SelectListItem{ Text=StyleDetails.RoleCustomer, Value=StyleDetails.RoleCustomer}
            };
            ViewBag.RoleList = roles;
            return View(regDto);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        /** SignIn a user using Dotnet Identity  */
        private async Task SignInUser(LoginResDto loginRes)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecToken = tokenHandler.ReadJwtToken(loginRes.Token);
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwtSecToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwtSecToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwtSecToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name, jwtSecToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwtSecToken.Claims.FirstOrDefault(u => u.Type == "role").Value));

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
