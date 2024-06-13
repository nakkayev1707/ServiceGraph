using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using QuickGraph;
using QuickGraph.Graphviz;
using ServiceGraph.Graph;
using ServiceGraph.Vizualization;

namespace ServiceGraph.Core;

internal class DependencyResolver
{
    private ServiceGraphOption? _graphOption;
    private GraphvizAlgorithm<Type, Edge<Type>> _graphviz;
    private readonly GraphVisualizer _graphVisualizer = new();

    public void ApplyOptions(ServiceGraphOption? option)
    {
        _graphOption = option;
        _graphVisualizer.SetOptions();
    }
    
    public void AddResolver(IServiceCollection services)
    {
        Dictionary<Type, List<Type>> servicesDict = Resolve(services);

        var dependencyGraphBuilder = new DependencyGraphBuilder();
        _graphviz = dependencyGraphBuilder.BuildGraph(servicesDict);
        _graphVisualizer.Visualize(_graphviz);
    }
    
    private Dictionary<Type, List<Type>> Resolve(IEnumerable<ServiceDescriptor> serviceDescriptors)
    {
        var dependencies = new Dictionary<Type, List<Type>>();
        foreach (ServiceDescriptor serviceDescriptor in serviceDescriptors)
        {
            Type serviceType = serviceDescriptor.ServiceType;
            Type? implementationType = serviceDescriptor.ImplementationType;

            if (implementationType != null)
            {
                ConstructorInfo? ctor = implementationType.GetConstructors().FirstOrDefault();
                if (ctor != null)
                {
                    List<Type> parameterTypes = ctor.GetParameters().Select(p => p.ParameterType).ToList();
                    dependencies[serviceType] = parameterTypes;
                }
            }
        }

        return dependencies;
    }
}