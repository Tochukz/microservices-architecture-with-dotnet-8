namespace Mango.Services.EmailAPI.Models.Dto
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; } = new CartHeaderDto();
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
    }
}
