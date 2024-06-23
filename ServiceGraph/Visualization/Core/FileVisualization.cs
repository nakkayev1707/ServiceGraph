using QuickGraph;
using QuickGraph.Graphviz;
using ServiceGraph.Graph;

namespace ServiceGraph.Visualization.Core;

public class FileVisualization : IVisualize
{
    private readonly GraphvizAlgorithm<Type, Edge<Type>> _graphviz;
    private readonly CycleDetector _cycleDetector;
    
    public FileVisualization(GraphvizAlgorithm<Type, Edge<Type>> graphviz)
    {
        _graphviz = graphviz;
        _cycleDetector = new CycleDetector(graphviz);
    }

    public void Visualize()
    {
        
    }
}