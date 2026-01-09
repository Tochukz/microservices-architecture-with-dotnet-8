namespace Mango.Services.ProductAPI.Models.Dto
{
    public class ResponseDto
    {
        public object? Result { set; get; }
        public bool IsSuccess { set; get; } = true;
        public string Message { set; get; } = "";
    }
}
