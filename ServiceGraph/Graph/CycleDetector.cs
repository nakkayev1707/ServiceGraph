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

    public Tuple<Type, Type>? TryFindCircularDependentServices()
    {
        if (_graphviz == null) throw new ArgumentNullException(nameof(_graphviz));
    
        IEdgeListGraph<Type, Edge<Type>>? graph = _graphviz.VisitedGraph;

        var visited = new Dictionary<Type, bool>();
        var inStack = new Dictionary<Type, bool>();

        foreach (Type? node in graph.Vertices)
        {
            visited[node] = false;
            inStack[node] = false;
        }
        
        foreach (Type? node in graph.Vertices)
        {
            if (!visited[node] && DFS(visited, node, inStack, graph))
            {
                Tuple<Type, Type>? cycleNodes = GetCycleNodes(node, inStack);
                if (cycleNodes != null)
                {
                    return new Tuple<Type, Type>(cycleNodes.Item1, cycleNodes.Item2);
                }
            }
        }

        return null;
    }
    
    private Tuple<Type, Type>? GetCycleNodes(Type startNode, Dictionary<Type, bool> inStack)
    {
        foreach (Type node in inStack.Keys)
        {
            if (inStack[node])
            {
                return new Tuple<Type, Type>(startNode, node);
            }
        }
        return null;
    }

    private bool DFS(Dictionary<Type, bool> visited, Type node, Dictionary<Type, bool> inStack, IEdgeListGraph<Type, Edge<Type>>? graph)
    {
        if (!visited[node])
        {
            visited[node] = true;
            inStack[node] = true;
            
            foreach (var edge in graph.Edges)
            {
                if (EqualityComparer<Type>.Default.Equals(edge.Source, node))
                {
                    Type? neighbor = edge.Target;

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