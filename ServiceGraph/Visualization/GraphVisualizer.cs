using QuickGraph;
using QuickGraph.Graphviz;
using ServiceGraph.Visualization.Core;

namespace ServiceGraph.Visualization;

public class GraphVisualizer
{
    private GraphVisualizationOption? _visualizationOption;

    public void SetOptions(GraphVisualizationOption optionVisualizationOption)
    {
        _visualizationOption = optionVisualizationOption;
    }

    public void Visualize(GraphvizAlgorithm<Type, Edge<Type>> graphviz)
    {
        switch (_visualizationOption?.VisualizationMethod)
        {
            case VisualizationMethod.Console:
            {
                new ConsoleVisualization(graphviz).Visualize();
                break;
            }
            default:
            {
                new ConsoleVisualization(graphviz).Visualize();
                break;
            }
        }
    }
}