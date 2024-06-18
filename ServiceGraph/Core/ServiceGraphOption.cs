using ServiceGraph.Visualization;

namespace ServiceGraph.Core;

public class ServiceGraphOption
{
    /// <summary>
    /// Namespaces to include while resolving dependencies
    /// If not null all other namespaces will be excluded from scan
    /// </summary>
    public string[] Namespaces { get; set; }
    public GraphVisualizationOption VisualizationOption { get; set; } }