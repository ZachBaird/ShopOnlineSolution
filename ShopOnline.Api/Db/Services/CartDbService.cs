using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Db.Entities;
using ShopOnline.Api.Infrastructure;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Db.Services;

public class CartDbService : IDiSingleton
{
    private readonly IAppServiceScopeFactory<AppDbContext> _scopeFactory;
    private readonly CartVerificationService _cartVerificationService;
    private readonly ProductDbService _productDbService;

    public CartDbService(IAppServiceScopeFactory<AppDbContext> scopeFactory,
        CartVerificationService cartVerificationService,
        ProductDbService productDbService)
    {
        _scopeFactory = scopeFactory;
        _cartVerificationService = cartVerificationService;
        _productDbService = productDbService;
    }

    /// <summary>
    /// Gets the <see cref="Cart"/> with the passed in id.
    /// </summary>
    public async Task<Cart> GetCartByIdAsync(int id)
    {
        var scope = _scopeFactory.CreateScope();
        var db = scope.GetRequiredService();

        return await db.Carts.FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Gets a list of <see cref="CartItem"/> in the system by the cartId passed in.
    /// </summary>
    public async Task<List<CartItemDto>> GetCartItemsByCartAsync(int cartId)
    {
        var scope = _scopeFactory.CreateScope();
        var db = scope.GetRequiredService();

        var cartItems = await db.CartItems.Where(ci => ci.CartId == cartId).ToListAsync();
        var products = await _productDbService.GetProductsAsync();

        var results = (from cartItem in cartItems
                      join product in products
                      on cartItem.ProductId equals product.Id
                      select new CartItemDto
                      {
                          Id = cartItem.Id,
                          ProductId = cartItem.ProductId,
                          ProductName = product.Name,
                          ProductDescription = product.Description,
                          CartId = cartItem.CartId,
                          Price = product.Price,
                          Qty = product.Qty,
                      }).ToList();

        return results;
    }

    public async Task<List<CartItemDto>> GetCartItemsByUserAsync(int userId)
    {
        var scope = _scopeFactory.CreateScope();
        var db = scope.GetRequiredService();

        #region Method style join
        //var result = await db.Carts
        //    .Where(c => c.UserId == userId)
        //    .Join(
        //        db.CartItems,
        //        cart => cart.Id,
        //        cartItem => cartItem.CartId,
        //        (cart, cartItem) =>
        //            new CartItem
        //            {
        //                Id = cartItem.Id,
        //                ProductId = cartItem.ProductId,
        //                Qty = cartItem.Qty,
        //                CartId = cart.Id
        //            })
        //    .ToListAsync();
        #endregion

        // Query syntax is more readable for joins.
        var cartItems = await (from cart in db.Carts
                           join cartItem in db.CartItems
                           on cart.Id equals cartItem.CartId
                           where cart.UserId == userId
                           select new CartItem
                           {
                               Id = cartItem.Id,
                               ProductId = cartItem.ProductId,
                               Qty = cartItem.Qty,
                               CartId = cart.Id
                           }).ToListAsync();

        if (cartItems == null)
            throw new ArgumentException($"No cart items for User '{userId}'.");

        var products = await _productDbService.GetProductsAsync();

        var results = (from cartItem in cartItems
                       join product in products
                       on cartItem.ProductId equals product.Id
                       select new CartItemDto
                       {
                           Id = cartItem.Id,
                           ProductId = cartItem.ProductId,
                           ProductName = product.Name,
                           ProductDescription = product.Description,
                           CartId = cartItem.CartId,
                           Price = product.Price,
                           Qty = product.Qty,
                       }).ToList();

        return results;
    }

    public async Task<CartItem> GetCartItemAsync(int id)
    {
        var scope = _scopeFactory.CreateScope();
        var db = scope.GetRequiredService();

        return await (from cart in db.Carts
                      join cartItem in db.CartItems
                      on cart.Id equals cartItem.CartId
                      where cartItem.Id == id
                      select new CartItem
                      {
                          Id = cartItem.Id,
                          ProductId = cartItem.ProductId,
                          Qty = cartItem.Qty,
                          CartId = cart.Id
                      }).SingleOrDefaultAsync();
    }

    public async Task<CartItem> AddItemToCartAsync(CartItemToAddDto cartItemToAdd)
    {
        var scope = _scopeFactory.CreateScope();
        var db = scope.GetRequiredService();

        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == cartItemToAdd.ProductId);
        if (product == null)
            throw new ArgumentException($"{nameof(cartItemToAdd.ProductId)} '{cartItemToAdd.ProductId}' does not exist.");

        var itemAlreadyInCart = await _cartVerificationService.CheckIfCartItemAlreadyExists(cartItemToAdd.CartId, product.Id);
        if (itemAlreadyInCart)
            return await db.CartItems.FirstAsync(ci => ci.CartId == cartItemToAdd.CartId && ci.ProductId == product.Id);

        var newCartItem = new CartItem()
        {
            CartId = cartItemToAdd.CartId,
            ProductId = product.Id,
            Qty = cartItemToAdd.Qty
        };

        var result = await db.CartItems.AddAsync(newCartItem);
        await db.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<CartItem> UpdateQtyAsync(int id, CartItemQtyUpdateDto cartItemToUpdate)
    {
        var scope = _scopeFactory.CreateScope();
        var db = scope.GetRequiredService();

        throw new NotImplementedException();
    }

    public async Task<CartItem> DeleteItemAsync(int id)
    {
        var scope = _scopeFactory.CreateScope();
        var db = scope.GetRequiredService();

        var itemToDelete = await db.CartItems.FirstOrDefaultAsync(ci => ci.Id == id);
        if (itemToDelete == null)
            throw new ArgumentException($"Cart Item '{id}' does not exist.");

        db.CartItems.Remove(itemToDelete);
        await db.SaveChangesAsync();
        return itemToDelete;
    }

}
