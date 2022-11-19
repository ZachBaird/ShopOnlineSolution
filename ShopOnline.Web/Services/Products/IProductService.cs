using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Products;

public interface IProductService
{
    Task<List<ProductDto>> GetProductsAsync();
    Task<ProductDto> GetProductByIdAsync(int id);
}
