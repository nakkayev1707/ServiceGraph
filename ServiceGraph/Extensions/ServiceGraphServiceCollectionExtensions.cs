using Microsoft.Extensions.DependencyInjection;
using ServiceGraph.Core;

namespace ServiceGraph.Extensions;

public static class ServiceGraphServiceCollectionExtensions
{
    public static IServiceCollection AddServiceGraph(this IServiceCollection services, ServiceGraphOption? options = null)
    {
        new DependencyResolver(services, options).Visualize();
        
        return services;
    }
}