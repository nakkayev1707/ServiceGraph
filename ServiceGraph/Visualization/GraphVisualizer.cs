﻿using QuickGraph;
using QuickGraph.Graphviz;
using ServiceGraph.Visualization.Core;

namespace ServiceGraph.Visualization;

public class GraphVisualizer
{
    private GraphVisualizationOption? _visualizationOption;

    public void SetOptions(GraphVisualizationOption optionVisualizationOption)
    {
        _visualizationOption = optionVisualizationOption;
    }

    public void Visualize(GraphvizAlgorithm<Type, Edge<Type>> graphviz)
    {
        IVisualize visualize = _visualizationOption?.VisualizationMethod switch
        {
            VisualizationMethod.Console => new ConsoleVisualization(graphviz),
            VisualizationMethod.File => new FileVisualization(graphviz),
            _ => new ConsoleVisualization(graphviz)
        };
        
        visualize.Visualize();
    }
}