using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StyleDetails.ApiType.POST,
                Data = couponDto,
                Url = StyleDetails.CouponAPIBase + "/api/coupon",
            });
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StyleDetails.ApiType.DELETE,
                Url = StyleDetails.CouponAPIBase + "/api/coupon/" + id,
            });
        }

        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StyleDetails.ApiType.GET,
                Url = StyleDetails.CouponAPIBase + "/api/coupon",
            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StyleDetails.ApiType.GET,
                Url = StyleDetails.CouponAPIBase + "/api/coupon/GetByCode/" + couponCode,
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StyleDetails.ApiType.GET,
                Url = StyleDetails.CouponAPIBase + "/api/coupon/" + id,
            });
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StyleDetails.ApiType.PUT,
                Data = couponDto,
                Url = StyleDetails.CouponAPIBase + "/api/coupon",
            });
        }
    }
}
