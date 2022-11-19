using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Db.Entities;
using ShopOnline.Api.Infrastructure;

namespace ShopOnline.Api.Db.Services;

public class CartDbService : IDiSingleton
{
    private readonly IAppServiceScopeFactory<AppDbContext> _scopeFactory;

    public CartDbService(IAppServiceScopeFactory<AppDbContext> scopeFactory)
    {
        _scopeFactory = scopeFactory;
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
    public async Task<List<CartItem>> GetCartItemsByCartAsync(int cartId)
    {
        var scope = _scopeFactory.CreateScope();
        var db = scope.GetRequiredService();

        return await db.CartItems.Where(ci => ci.CartId == cartId).ToListAsync();
    }
}
