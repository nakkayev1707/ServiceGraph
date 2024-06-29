using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace ServiceGraph.Visualization.Core;

public class FileDotEngine : IDotEngine
{
    public string Run(GraphvizImageType imageType, string dot, string outputFileName)
    {
        using (var writer = new StreamWriter(outputFileName))
        {
            writer.Write(dot);    
        }

        return Path.GetFileName(outputFileName);
    }
}