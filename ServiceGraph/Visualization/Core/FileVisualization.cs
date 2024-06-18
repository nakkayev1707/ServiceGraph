using QuickGraph;
using QuickGraph.Graphviz;

namespace ServiceGraph.Visualization.Core;

public class FileVisualization : IVisualize
{
    private readonly GraphvizAlgorithm<Type, Edge<Type>> _graphviz;
    
    public FileVisualization(GraphvizAlgorithm<Type, Edge<Type>> graphviz)
    {
        _graphviz = graphviz;
    }

    public void Visualize()
    {
        throw new NotImplementedException();
    }
}