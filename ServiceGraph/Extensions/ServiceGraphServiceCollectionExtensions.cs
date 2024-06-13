using Microsoft.Extensions.DependencyInjection;
using ServiceGraph.Core;

namespace ServiceGraph.Extensions;

public static class ServiceGraphServiceCollectionExtensions
{
    public static IServiceCollection AddServiceGraph(this IServiceCollection services, ServiceGraphOption? options = null)
    {
        var dependencyResolver = new DependencyResolver();
        if (options != null)
        {
            dependencyResolver.ApplyOptions(options);
        }
        dependencyResolver.AddResolver(services);
        return services;
    }
}