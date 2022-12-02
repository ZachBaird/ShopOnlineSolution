namespace ShopOnline.Web.Core;

public static class ApiPaths
{
    public const string GetProductsList = "api/product/list";
    public static string GetProduct(int id) => $"api/product/{id}";
}
