namespace Mango.Services.AuthAPI.Models
{
    public class JwtOptions
    {
        public string Issuer { set; get; } = string.Empty;
        public string Audience { set; get; } = string.Empty;
        public string Secret { set; get; } = string.Empty;
    }
}
