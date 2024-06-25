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

    public Type? TryFindCircularDependentServices()
    {
        if (_graphviz == null) throw new ArgumentNullException(nameof(_graphviz));
    
        IEdgeListGraph<Type, Edge<Type>>? graph = _graphviz.VisitedGraph;

        // Dictionary to keep track of visited nodes and their state
        Dictionary<Type, bool> visited = new Dictionary<Type, bool>();
        Dictionary<Type, bool> inStack = new Dictionary<Type, bool>();

        foreach (var node in graph.Vertices)
        {
            visited[node] = false;
            inStack[node] = false;
        }
        
        foreach (var node in graph.Vertices)
        {
            if (DFS(visited, node, inStack, graph))
            {
                return node;
            }
        }

        return null;
    }

    private static bool DFS(Dictionary<Type, bool> visited, Type node, Dictionary<Type, bool> inStack, IEdgeListGraph<Type, Edge<Type>>? graph)
    {
        if (!visited[node])
        {
            visited[node] = true;
            inStack[node] = true;
            
            foreach (var edge in graph.Edges)
            {
                if (EqualityComparer<Type>.Default.Equals(edge.Source, node))
                {
                    var neighbor = edge.Target;

                    if (!visited[neighbor] && DFS(visited, neighbor, inStack, graph))
                    {
                        return true;
                    }
                    if (inStack[neighbor])
                    {
                        return true;
                    }
                }
            }
        }

        inStack[node] = false;
        return false;
    }
}