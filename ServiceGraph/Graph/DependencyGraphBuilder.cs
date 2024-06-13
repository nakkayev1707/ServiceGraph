using QuickGraph;
using QuickGraph.Graphviz;

namespace ServiceGraph.Graph;

internal class DependencyGraphBuilder
{
    public void BuildGraph(Dictionary<Type, List<Type>> dependencies)
    {
        var graph = new AdjacencyGraph<Type, Edge<Type>>();

        foreach (KeyValuePair<Type,List<Type>> dependency in dependencies)
        {
            Type serviceType = dependency.Key;
            graph.AddVertex(serviceType);

            foreach (Type dependencyType in dependency.Value)
            {
                graph.AddVerticesAndEdge(new Edge<Type>(serviceType, dependencyType));
            }
        }
        
        var graphviz = new GraphvizAlgorithm<Type, Edge<Type>>(graph);
        
        graphviz.FormatVertex += (sender, args) =>
        {
            args.VertexFormatter.Label = args.Vertex.FullName;
        };

        graphviz.FormatEdge += (sender, args) =>
        {
            args.EdgeFormatter.Label.Value = string.Empty;
        };
        
        string dot = graphviz.Generate();
        File.WriteAllText("dependencyGraph.dot", dot);
    }
}