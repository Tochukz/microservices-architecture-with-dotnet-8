using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            HttpClient client = _httpClientFactory.CreateClient("Coupon");
            HttpResponseMessage responseMsg = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");
            string responseStr = await responseMsg.Content.ReadAsStringAsync();
            ResponseDto responseDto = JsonConvert.DeserializeObject<ResponseDto>(responseStr);
            if (responseDto.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));
            }
            return new CouponDto();
        }
    }
}
