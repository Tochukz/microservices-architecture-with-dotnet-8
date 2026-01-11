using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        IBaseService _baseService;
        public ProductService(IBaseService service)
        {
            _baseService = service;
        }

        public async Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StyleDetails.ApiType.POST,
                Data = productDto,
                Url = StyleDetails.ProductAPIBase + "/api/product"
            });
        }

        public Task<ResponseDto?> DeleteProductAsync(int id)
        {
           return _baseService.SendAsync(new RequestDto
            {
                ApiType = StyleDetails.ApiType.DELETE,
                Url = StyleDetails.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto?> GetProductAsync(int id)
        {
           return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StyleDetails.ApiType.GET,
                Url = StyleDetails.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto?> GetProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StyleDetails.ApiType.GET,
                Url = StyleDetails.ProductAPIBase + "/api/product"
            });
        }

        public async Task<ResponseDto?> UpateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StyleDetails.ApiType.PUT,
                Data = productDto,
                Url = StyleDetails.ProductAPIBase + "/api/product"
            });
        }
    }
}
