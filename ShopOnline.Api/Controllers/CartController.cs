using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Db.Entities;
using ShopOnline.Api.Db.Services;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers;

[ApiController]
[Route("api/carts")]
public class CartController : ControllerBase
{
    private readonly CartDbService _cartDbService;

    public CartController(CartDbService cartDbService)
    {
        _cartDbService = cartDbService;
    }

    [HttpGet("user/list/{id:int}")]
    public async Task<ActionResult<List<CartItemDto>>> GetCartItemsByUser(int id)
    {
        var results = await _cartDbService.GetCartItemsByUserAsync(id);
        return Ok(results);
    }

    [HttpGet("cart/list/{id:int}")]
    public async Task<ActionResult<List<CartItemDto>>> GetCartItemsByCart(int id)
    {
        var results = await _cartDbService.GetCartItemsByCartAsync(id);
        return Ok(results);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Cart>> GetCart(int id)
    {
        var result = await _cartDbService.GetCartByIdAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("cart/{id:int}")]
    public async Task<ActionResult<CartItem>> GetCartItem(int id)
    {
        var result = await _cartDbService.GetCartItemAsync(id);
        if (result == null)
            return NoContent();

        return Ok(result);
    }
}
