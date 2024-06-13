using Microsoft.Extensions.DependencyInjection;
using ServiceGraph.Core;
using ServiceGraph.Graph;

namespace ServiceGraph.Extensions;

public static class ServiceGraphServiceCollectionExtensions
{
    public static IServiceCollection AddServiceGraph(
        this IServiceCollection services)
    {
        var dependencyResolver = new DependencyResolver();
        Dictionary<Type, List<Type>> servicesDict = dependencyResolver.Resolve(services);

        var dependencyGraphBuilder = new DependencyGraphBuilder();
        dependencyGraphBuilder.BuildGraph(servicesDict);
        
        return services;
    }
}