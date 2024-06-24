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

        // Recursive function to perform DFS and detect cycles
        bool DFS(Type node)
        {
            if (!visited[node])
            {
                visited[node] = true;
                inStack[node] = true;

                // Iterate through all edges to find outgoing edges for the current node
                foreach (var edge in graph.Edges)
                {
                    if (EqualityComparer<Type>.Default.Equals(edge.Source, node))
                    {
                        var neighbor = edge.Target;

                        if (!visited[neighbor] && DFS(neighbor))
                        {
                            return true;
                        }
                        else if (inStack[neighbor])
                        {
                            return true;
                        }
                    }
                }
            }

            inStack[node] = false;
            return false;
        }

        // Perform DFS for each node
        foreach (var node in graph.Vertices)
        {
            if (DFS(node))
            {
                return true;
            }
        }

        return false;
    }

}