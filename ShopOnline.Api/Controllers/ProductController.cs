using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Db.Services;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers;

[ApiController]
[Route("api/product")]
public class ProductController : ControllerBase
{
    private readonly ProductDbService _productDbService;

    public ProductController(ProductDbService productDbService)
    {
        _productDbService = productDbService;
    }

    [HttpGet]
    [Route("/list")]
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        var products = await _productDbService.GetProductsAsync();
        var results = products.Select(async p => new ProductDto()
        { 
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            CategoryId = p.CategoryId,
            CategoryName = (await _productDbService.GetCategoryByIdAsync(p.CategoryId)).Name,
            Qty = p.Qty,
            ImageUrl = p.ImageUrl,
            Price = p.Price,
        }).ToList();

        return Ok(results);
    }

    [HttpGet]
    [Route("/{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct([FromQuery] int productId)
    {
        var product = await _productDbService.GetProductByIdAsync(productId);
        return Ok(new ProductDto()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            CategoryId = product.CategoryId,
            CategoryName = (await _productDbService.GetCategoryByIdAsync(product.CategoryId)).Name,
            Qty = product.Qty,
            ImageUrl = product.ImageUrl,
            Price = product.Price
        });
    }
}
