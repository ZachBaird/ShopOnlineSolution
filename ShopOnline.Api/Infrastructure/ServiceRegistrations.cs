using System.Reflection;

namespace ShopOnline.Api.Infrastructure;

public interface IDiSingleton { }
public interface IDiScoped { }
public interface IDiTransient { }

public static class ServiceRegistrations
{
    public static void RegisterServices(this IServiceCollection services)
    {
        var result = (from type in Assembly.GetExecutingAssembly().GetTypes()
                      where (type.GetInterfaces().Contains(typeof(IDiSingleton))
                        || type.GetInterfaces().Contains(typeof(IDiScoped))
                        || type.GetInterfaces().Contains(typeof(IDiTransient)))
                      select type).ToList();

        foreach (var service in result)
        {
            if (service.GetInterfaces().Contains(typeof(IDiSingleton)))
                services.AddSingleton(service);
            else if (service.GetInterfaces().Contains(typeof(IDiScoped)))
                services.AddScoped(service);
            else if (service.GetInterfaces().Contains(typeof(IDiTransient)))
                services.AddTransient(service);
        }
    }
}
