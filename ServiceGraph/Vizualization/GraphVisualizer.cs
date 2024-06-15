using QuickGraph;
using QuickGraph.Graphviz;

namespace ServiceGraph.Vizualization;

public class GraphVisualizer
{
    private GraphVisualizationOption _visualizationOption;

    public void SetOptions(GraphVisualizationOption optionVisualizationOption)
    {
        _visualizationOption = optionVisualizationOption;
    }

    public void Visualize(GraphvizAlgorithm<Type, Edge<Type>> graphviz)
    {
        throw new NotImplementedException();
    }
}