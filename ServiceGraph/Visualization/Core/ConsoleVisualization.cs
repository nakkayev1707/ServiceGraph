using QuickGraph;
using QuickGraph.Graphviz;
using Spectre.Console;

namespace ServiceGraph.Visualization.Core;

public class ConsoleVisualization : IVisualize
{
    private readonly GraphvizAlgorithm<Type, Edge<Type>> _graphviz;
    
    public ConsoleVisualization( GraphvizAlgorithm<Type, Edge<Type>> graphviz)
    {
        _graphviz = graphviz;
    }

    public void Visualize()
    {
        string dot = _graphviz.Generate();
        AnsiConsole.MarkupLine("[bold yellow]Generated Graphviz DOT Representation:[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold green]----------------------------------------[/]");
        AnsiConsole.WriteLine(dot);
        AnsiConsole.MarkupLine("[bold green]----------------------------------------[/]");
    }
}