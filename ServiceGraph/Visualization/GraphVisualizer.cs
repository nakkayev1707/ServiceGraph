using QuickGraph;
using QuickGraph.Graphviz;
using ServiceGraph.Visualization.Core;

namespace ServiceGraph.Visualization;

public class GraphVisualizer
{
    private readonly GraphVisualizationOption? _visualizationOption;
    private readonly GraphvizAlgorithm<Type, Edge<Type>> _graphviz;

    public GraphVisualizer(GraphvizAlgorithm<Type, Edge<Type>> graphviz, GraphVisualizationOption? visualizationOption)
    {
        _graphviz = graphviz;
        _visualizationOption = visualizationOption ?? new GraphVisualizationOption
        {
            VisualizationMethod = VisualizationMethod.Console
        };
    }

    public void Visualize()
    {
        IVisualize visualizationObj = _visualizationOption?.VisualizationMethod switch
        {
            VisualizationMethod.Console => new ConsoleVisualization(_graphviz),
            VisualizationMethod.File => new FileVisualization(_graphviz),
            _ => new ConsoleVisualization(_graphviz)
        };
        
        visualizationObj.Visualize();
    }
}