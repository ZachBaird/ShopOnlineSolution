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

    [HttpGet("list")]
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        var products = await _productDbService.GetProductsAsync();
        if (products.Count == 0)
            return NotFound();

        var productCategories = await _productDbService.GetCategoriesAsync();

        var results = products.Select(p => new ProductDto()
        { 
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            CategoryId = p.CategoryId,
            CategoryName = productCategories.First(pc => pc.Id == p.CategoryId).Name,
            Qty = p.Qty,
            ImageUrl = p.ImageUrl,
            Price = p.Price,
        }).ToList();

        return Ok(results);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _productDbService.GetProductByIdAsync(id);
        if (product == null)
            return NoContent();

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
