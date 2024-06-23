﻿using QuickGraph;
using QuickGraph.Graphviz;
using ServiceGraph.Graph;
using Spectre.Console;

namespace ServiceGraph.Visualization.Core;

public class ConsoleVisualization : IVisualize
{
    private readonly GraphvizAlgorithm<Type, Edge<Type>> _graphviz;
    private readonly CycleDetector _cycleDetector;
    
    public ConsoleVisualization( GraphvizAlgorithm<Type, Edge<Type>> graphviz)
    {
        _graphviz = graphviz;
        _cycleDetector = new CycleDetector(graphviz);
    }

    public void Visualize()
    {
        string dot = _graphviz.Generate();
        AnsiConsole.MarkupLine("[bold yellow]Generated Graphviz DOT Representation:[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold green]----------------------------------------[/]");
        AnsiConsole.WriteLine(dot);
        if (_cycleDetector.HasCycle())
        {
            AnsiConsole.MarkupLine("[bold red] cycle detected[/]");
        }
        AnsiConsole.MarkupLine("[bold green]----------------------------------------[/]");
    }
}