using Microsoft.AspNetCore.Identity;
using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Mango.Services.AuthAPI.Models;


namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            ApplicationUser appUser = _db.ApplicationUsers.FirstOrDefault(usr => usr.Email == email);
            if (appUser != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    /* Create role if not exist */
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(appUser, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResDto> Login(LoginReqDto loginReqDto)
        {
            ApplicationUser appUser = _db.ApplicationUsers.FirstOrDefault(usr => usr.UserName.ToLower() == loginReqDto.UserName);
            bool isValid = await _userManager.CheckPasswordAsync(appUser, loginReqDto.Password);
            if (appUser  == null || !isValid )
            {
                return new LoginResDto { User = null, Token = "" };
            }

            string token = _jwtTokenGenerator.GenerateToken(appUser);
            UserDto userDto = new()
            {
                Email = appUser.Email,
                ID = appUser.Id,
                Name = appUser.Name,
                PhoneNumber = appUser.PhoneNumber,
            };

            LoginResDto loginResDto = new()
            {
                User = userDto,
                Token = token
            };

            return loginResDto;
        }

        public async Task<string> Register(RegistrationReqDto registrationReqDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationReqDto.Email,
                Email = registrationReqDto.Email,
                NormalizedEmail = registrationReqDto.Email.ToUpper(),
                Name = registrationReqDto.Name,
                PhoneNumber =registrationReqDto.PhoneNumber,
            };

            try
            {
                IdentityResult result = await _userManager.CreateAsync(user, registrationReqDto.Password);
                if (result.Succeeded)
                {
                    ApplicationUser appUser = _db.ApplicationUsers.First(usr => usr.UserName == registrationReqDto.Email);
                    UserDto userDto = new()
                    {
                        Email = appUser.Email,
                        ID = appUser.Id,
                        Name = appUser.Name,
                        PhoneNumber = appUser.PhoneNumber

                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch(Exception ex)
            {

            }

            return "Error Encountered";
        }
    }
}
