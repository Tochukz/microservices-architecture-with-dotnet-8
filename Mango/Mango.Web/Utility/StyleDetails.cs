namespace Mango.Web.Utility
{
    public class StyleDetails
    {
        public static string CouponAPIBase { set; get; }
        public static string AuthAPIBase { set; get; }
        public static string ProductAPIBase { set; get; }
        public const string RoleAdmin = "ADMIN";

        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JWTToken";
        public enum ApiType
        {
            GET, 
            POST, 
            PUT, 
            DELETE
        }
    }
}
