namespace ShopOnline.Api.Infrastructure;

public interface IAppServiceScope<T> : IDisposable where T : class
{
    T GetRequiredService();

    T GetService();

    IEnumerable<T> GetServices();


}

public class AppServiceScope<T> : IAppServiceScope<T> where T : class
{
    private readonly IServiceScope _scope;

    public AppServiceScope(IServiceScope scope) =>
        _scope = scope;

    public T GetRequiredService() =>
        _scope.ServiceProvider.GetRequiredService<T>();

    public T GetService() =>
        _scope.ServiceProvider.GetService<T>();

    public IEnumerable<T> GetServices() =>
        _scope.ServiceProvider.GetServices<T>();
    
    private bool _disposed = false;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool calledFromCodeNotTheGarbageCollector)
    {
        if (_disposed)
            return;
        if (calledFromCodeNotTheGarbageCollector)
            _scope?.Dispose();
        _disposed = true;
    }
    ~AppServiceScope() { Dispose(false); }
}
