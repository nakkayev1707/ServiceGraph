using Microsoft.Extensions.DependencyInjection;
using QuickGraph;
using QuickGraph.Graphviz;
using ServiceGraph.Graph;

namespace ServiceGraph.Core;

internal class DependencyResolver
{
    public DependencyResolver(IServiceCollection services, ServiceGraphOption? option)
    {
        var builder = new DependencyGraphBuilder(services, option);
        GraphvizAlgorithm<Type, Edge<Type>> graphviz = builder.BuildGraph();
    }

    public void Resolve()
    {
        
    }
}