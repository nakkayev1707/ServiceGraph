using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using QuickGraph;
using QuickGraph.Graphviz;
using ServiceGraph.Core;

namespace ServiceGraph.Graph;

internal class DependencyGraphBuilder
{
    private readonly IServiceCollection _serviceCollection;
    private readonly ServiceGraphOption? _graphOption;

    public DependencyGraphBuilder(IServiceCollection serviceCollection, ServiceGraphOption? graphOption)
    {
        _serviceCollection = serviceCollection;
        _graphOption = graphOption;
    }
    
    public GraphvizAlgorithm<Type, Edge<Type>> BuildGraph()
    {
        Dictionary<Type, List<Type>> dependencies = ResolveDependencies(_serviceCollection, _graphOption);
        
        var graph = new AdjacencyGraph<Type, Edge<Type>>();

        foreach (KeyValuePair<Type,List<Type>> dependency in dependencies)
        {
            Type serviceType = dependency.Key;
            graph.AddVertex(serviceType);

            foreach (Type dependencyType in dependency.Value)
            {
                graph.AddVerticesAndEdge(new Edge<Type>(serviceType, dependencyType));
            }
        }
        
        var graphviz = new GraphvizAlgorithm<Type, Edge<Type>>(graph);
        
        graphviz.FormatVertex += (sender, args) =>
        {
            args.VertexFormatter.Label = args.Vertex.FullName;
        };

        graphviz.FormatEdge += (sender, args) =>
        {
            args.EdgeFormatter.Label.Value = string.Empty;
        };

        return graphviz;
    }
    
    private Dictionary<Type, List<Type>> ResolveDependencies(IServiceCollection services, ServiceGraphOption? graphOption)
    {
        var dependencies = new Dictionary<Type, List<Type>>();

        foreach (ServiceDescriptor serviceDescriptor in services)
        {
            Type serviceType = serviceDescriptor.ServiceType;
            Type? implementationType = serviceDescriptor.ImplementationType;

            if (implementationType != null && (graphOption?.Namespaces == null 
                                               || IsCustomNamespace(implementationType, graphOption.Namespaces)))
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
    
    private bool IsCustomNamespace(Type type, string[] customNamespaces)
    {
        return customNamespaces.Any(ns => type.Namespace != null && type.Namespace.StartsWith(ns));
    }
}