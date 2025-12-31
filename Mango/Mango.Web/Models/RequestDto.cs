using static Mango.Web.Utility.StyleDetails;

namespace Mango.Web.Models
{
    public class RequestDto
    {
        public ApiType ApiType { set; get; } = ApiType.GET;
        public string Url { set; get; }
        public object Data { set; get; }
        public string AccessToken { set; get; }

    }
}
 