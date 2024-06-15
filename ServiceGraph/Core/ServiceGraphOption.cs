using ServiceGraph.Vizualization;

namespace ServiceGraph.Core;

public class ServiceGraphOption
{
    public string[] Namespaces { get; set; }
    public GraphVisualizationOption VisualizationOption { get; set; }
}