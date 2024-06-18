using Microsoft.Extensions.DependencyInjection;
using QuickGraph;
using QuickGraph.Graphviz;
using ServiceGraph.Graph;
using ServiceGraph.Visualization;

namespace ServiceGraph.Core;

internal class DependencyResolver
{
    private readonly GraphVisualizer _graphVisualizer;
    private readonly GraphvizAlgorithm<Type, Edge<Type>> _graphviz;

    public DependencyResolver(IServiceCollection services, ServiceGraphOption? option)
    {
        var builder = new DependencyGraphBuilder();
        _graphviz = builder.BuildGraph(services, option);
        
        _graphVisualizer = new GraphVisualizer(_graphviz);
        if (option != null) _graphVisualizer.SetOptions(option.VisualizationOption);
    }

    public void Visualize()
    {
        _graphVisualizer.Visualize(_graphviz);
    }
}