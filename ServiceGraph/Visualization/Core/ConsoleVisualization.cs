using QuickGraph;
using QuickGraph.Graphviz;

namespace ServiceGraph.Visualization.Core;

public class ConsoleVisualization
{
    private readonly GraphvizAlgorithm<Type, Edge<Type>> _graphviz;
    
    public ConsoleVisualization( GraphvizAlgorithm<Type, Edge<Type>> graphviz)
    {
        _graphviz = graphviz;
    }

    public void Visualize()
    {
        string dot = _graphviz.Generate();
        File.WriteAllText("dependencyGraph.dot", dot);
    }
}