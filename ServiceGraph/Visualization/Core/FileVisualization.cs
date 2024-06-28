using System.Text;
using QuickGraph;
using QuickGraph.Graphviz;
using ServiceGraph.Graph;

namespace ServiceGraph.Visualization.Core;

public class FileVisualization : IVisualize
{
    private readonly GraphvizAlgorithm<Type, Edge<Type>> _graphviz;
    private readonly string _outputFileName;
    private readonly CycleDetector _cycleDetector;
    
    public FileVisualization(GraphvizAlgorithm<Type, Edge<Type>> graphviz, string outputFileName)
    {
        _graphviz = graphviz;
        _outputFileName = outputFileName;
        _cycleDetector = new CycleDetector(graphviz);
    }

    public void Visualize()
    {
        string dot = _graphviz.Generate();
        var logMessage = new StringBuilder(dot);
        
        Tuple<Type, Type>? circularServices = _cycleDetector.TryFindCircularDependentServices();
        if (circularServices != null)
        {
            logMessage.AppendLine($"[bold red] cycle detected: {circularServices.Item1.FullName} => {circularServices.Item2.FullName}");
        }
        
        File.WriteAllText(_outputFileName, logMessage.ToString());
    }
}