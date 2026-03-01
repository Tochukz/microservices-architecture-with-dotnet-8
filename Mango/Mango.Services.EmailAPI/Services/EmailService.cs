using Microsoft.EntityFrameworkCore;
using Mango.Services.EmailAPI.Models.Dto;
using Mango.Services.EmailAPI.Data;
using System.Text;
using Mango.Services.EmailAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("<br /> Cart Email Request ");
            message.AppendLine("<br /> Total " + cartDto.CartHeader.CartTotal);
            message.Append("<br />");
            message.Append("<ul>");
            foreach(var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);
        }

        public async Task EmailRegistrationAndLog(RegistrationReqDto registrationReqDto)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("<br /> Registration Email Request ");
            message.AppendLine("<br /> Email " + registrationReqDto.Email);
           
            await LogAndEmail(message.ToString(), "dotnetmastery@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };
                await using var _db = new AppDbContext(_dbOptions);
                await _db.EmailLogger.AddAsync(emailLog);
                await _db.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
