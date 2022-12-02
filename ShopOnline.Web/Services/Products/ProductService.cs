using ShopOnline.Models.Dtos;
using System.Net.Http.Json;
using Paths = ShopOnline.Web.Core.ApiPaths;

namespace ShopOnline.Web.Services.Products;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("ContentType", "applicaton/json");
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync(Paths.GetProduct(id));
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ProductDto>();
        }
        else
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new Exception("No product was found with this id!");

            var message = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Http status code: {response.StatusCode} message: {message}");
        }

    }

    public async Task<List<ProductDto>> GetProductsAsync()
    {
        var response = await _httpClient.GetAsync(Paths.GetProductsList);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        }
        else
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new Exception("No products in the system!");

            var message = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Http status code: {response.StatusCode} message: {message}");
        }
    }
}
