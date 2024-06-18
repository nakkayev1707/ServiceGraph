namespace ServiceGraph.Vizualization;

public class GraphVisualizationOption
{
    public VisualizationMethod VisualizationMethod { get; set; }

    /// <summary>
    /// Highlight issues like circular dependencies
    /// While highlighting visualization method will be used depends on <see cref="VisualizationMethod"/>
    /// </summary>
    public bool HighlightIssues { get; set; } = true;
}