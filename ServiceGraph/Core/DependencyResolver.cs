using Microsoft.Extensions.DependencyInjection;
using QuickGraph;
using QuickGraph.Graphviz;
using ServiceGraph.Graph;
using ServiceGraph.Visualization;

namespace ServiceGraph.Core;

internal class DependencyResolver
{
    private readonly GraphVisualizer _graphVisualizer;

    public DependencyResolver(IServiceCollection services, ServiceGraphOption? option)
    {
        var builder = new DependencyGraphBuilder(services, option);
        GraphvizAlgorithm<Type, Edge<Type>> graphviz = builder.BuildGraph();
        
        _graphVisualizer = new GraphVisualizer(graphviz, option?.VisualizationOption);
    }

    public void Visualize()
    {
        _graphVisualizer.Visualize();
    }
}