using QuickGraph;
using QuickGraph.Graphviz;

namespace ServiceGraph.Graph;

public class CycleDetector
{
    private readonly GraphvizAlgorithm<Type, Edge<Type>> _graphviz;

    public CycleDetector(GraphvizAlgorithm<Type, Edge<Type>> graphviz)
    {
        _graphviz = graphviz;
    }

    public bool HasCycle()
    {
        
        throw new NotImplementedException();
    }
}