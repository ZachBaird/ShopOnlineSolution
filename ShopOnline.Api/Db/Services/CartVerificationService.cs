using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Infrastructure;

namespace ShopOnline.Api.Db.Services;

public class CartVerificationService : IDiSingleton
{
    private readonly IAppServiceScopeFactory<AppDbContext> _scopeFactory;

    public CartVerificationService(IAppServiceScopeFactory<AppDbContext> scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<bool> CheckIfCartItemAlreadyExists(int cartId, int productId)
    {
        var scope = _scopeFactory.CreateScope();
        var db = scope.GetRequiredService();

        return await db.CartItems.AnyAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
    }
}
