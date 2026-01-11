using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetProductsAsync();
        Task<ResponseDto?> GetProductAsync(int id);
        Task<ResponseDto?> CreateProductAsync(ProductDto productDto);
        Task<ResponseDto?> UpateProductAsync(ProductDto productDto);
        Task<ResponseDto?> DeleteProductAsync(int id);


    }
}
