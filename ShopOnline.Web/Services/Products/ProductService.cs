using ShopOnline.Models.Dtos;
using System.Net.Http.Json;

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
        try
        {
            var response = await _httpClient.GetAsync($"api/product/{id}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return new ProductDto();

                return await response.Content.ReadFromJsonAsync<ProductDto>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} message: {message}");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<ProductDto>> GetProductsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/product/list");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return new List<ProductDto>();

                return await response.Content.ReadFromJsonAsync<List<ProductDto>>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} message: {message}");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
