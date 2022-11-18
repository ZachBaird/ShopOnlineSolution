namespace ShopOnline.Api.Infrastructure;

public interface IAppServiceScopeFactory<T> where T : class
{
    IAppServiceScope<T> CreateScope();
}

public class AppServiceScopeFactory<T> : IAppServiceScopeFactory<T> where T : class
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AppServiceScopeFactory(IServiceScopeFactory serviceScopeFactory) =>
        _serviceScopeFactory = serviceScopeFactory;


    public IAppServiceScope<T> CreateScope() =>
        new AppServiceScope<T>(_serviceScopeFactory.CreateScope());
}
