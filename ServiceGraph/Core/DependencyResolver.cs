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

    public void ApplyOptions(ServiceGraphOption option)
    {
        _graphOption = option ?? throw new ArgumentNullException(nameof(option));
        _graphVisualizer.SetOptions(option.VisualizationOption);
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

            if (implementationType != null && (_graphOption?.Namespaces == null 
                                               || IsCustomNamespace(implementationType, _graphOption.Namespaces)))
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
    
    
    /// <summary>
    /// Another method to resolve dependency graph, should be compared with used one <see cref="Resolve"/>
    /// and selected one of them
    /// </summary>
    private string GenerateDependencyGraph(IServiceCollection services)
    {
        var dependencies = new List<string>();

        foreach (var service in services)
        {
            var implementationType = service.ImplementationType ?? service.ServiceType;
            if (implementationType != null && IsCustomNamespace(implementationType, _graphOption.Namespaces))
            {
                foreach (var constructor in implementationType.GetConstructors())
                {
                    foreach (var parameter in constructor.GetParameters())
                    {
                        if (IsCustomNamespace(parameter.ParameterType, _graphOption.Namespaces))
                        {
                            var dependency = $"\"{implementationType.FullName}\" -> \"{parameter.ParameterType.FullName}\"";
                            dependencies.Add(dependency);
                        }
                    }
                }
            }
        }

        return "digraph G {\n" + string.Join(";\n", dependencies) + ";\n}";
    }
    
    private bool IsCustomNamespace(Type type, string[] customNamespaces)
    {
        return customNamespaces.Any(ns => type.Namespace != null && type.Namespace.StartsWith(ns));
    }
}