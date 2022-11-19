using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Db.Entities;
using ShopOnline.Api.Infrastructure;

namespace ShopOnline.Api.Db.Services;

/// <summary>
/// Singleton service for getting <see cref="Product"/> and <see cref="ProductCategory"/>
/// from the database.
/// </summary>
public class ProductDbService : IDiSingleton
{
    private readonly IAppServiceScopeFactory<AppDbContext> _scopeFactory;

    public ProductDbService(IAppServiceScopeFactory<AppDbContext> scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Returns a list of all the <see cref="ProductCategory"/> in the system.
    /// </summary>
    public async Task<List<ProductCategory>> GetCategoriesAsync()
    {
        var scope = _scopeFactory.CreateScope();
        var db = scope.GetRequiredService();

        return await db.ProductCategories.ToListAsync();
    }

    /// <summary>
    /// Returns a <see cref="ProductCategory"/> with the passed in id.
    /// </summary>
    public async Task<ProductCategory> GetCategoryByIdAsync(int id)
    {
        var scope = _scopeFactory.CreateScope();
        var db = scope.GetRequiredService();

        return await db.ProductCategories.FirstOrDefaultAsync(pc => pc.Id == id);
    }
    
    /// <summary>
    /// Gets a list of all the <see cref="Product"/> in the system.
    /// </summary>
    public async Task<List<Product>> GetProductsAsync()
    {
        var scope = _scopeFactory.CreateScope();
        var db = scope.GetRequiredService();

        return await db.Products.ToListAsync();
    }

    /// <summary>
    /// Gets a <see cref="Product"/> with the passed in id.
    /// </summary>
    public async Task<Product> GetProductByIdAsync(int id)
    {
        var scope = _scopeFactory.CreateScope();
        var db = scope.GetRequiredService();

        return await db.Products.FirstOrDefaultAsync(p => p.Id == id);
    }
}
