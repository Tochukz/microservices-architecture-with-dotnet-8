namespace Mango.Web.Models
{
    public class ResponseDto
    {
        public object? Result { set; get; }

        public bool IsSuccess { set; get; } = true;

        public string Message { set; get; } = "";
    }
}
