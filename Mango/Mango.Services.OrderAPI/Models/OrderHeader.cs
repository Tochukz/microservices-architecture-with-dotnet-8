using System.ComponentModel.DataAnnotations;

namespace Mango.Services.OrderAPI.Models
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double OrderTotal { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime OrderTime { set; get; }
        public string? Status { set; get; }
        public string? PaymentIntentId { set; get; }
        public string? StripeSessionId { set; get; }
        public IEnumerable<OrderDetails> OrderDetails { set; get; }

    }
}
