using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Products;

namespace ShopOnline.Web.Components.BaseComponents;

public class ProductsBase : ComponentBase
{
    [Inject]
    public IProductService ProductService { get; set; }

    public List<ProductDto> Products { get; set; }

    public ProductDto CurrentProduct { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Products = await ProductService.GetProductsAsync();
    }
}
