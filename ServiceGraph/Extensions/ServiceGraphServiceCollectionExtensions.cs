using Microsoft.Extensions.DependencyInjection;
using ServiceGraph.Core;
using ServiceGraph.Graph;

namespace ServiceGraph.Extensions;

public static class ServiceGraphServiceCollectionExtensions
{
    public static IServiceCollection AddServiceGraph(this IServiceCollection services)
    {
        new DependencyResolver().AddResolver(services);
        return services;
    }
}