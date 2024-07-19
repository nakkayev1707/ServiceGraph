using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using QuickGraph;
using QuickGraph.Graphviz;
using ServiceGraph.Core;
using ServiceGraph.Graph;

namespace ServiceGraph.Visualization.Core;

public class HtmlBuilder
{
    private const string TemplateFileName = "ServiceGraph.Visualization.Core.service-graph.html";
    private readonly DependencyGraphBuilder _dependencyGraphBuilder;
    
    public HtmlBuilder(ServiceGraphOption graphOption, ServiceCollection serviceCollection)
    {
        _dependencyGraphBuilder = new DependencyGraphBuilder(serviceCollection, graphOption);
    }
    
    public async Task<string> BuildAsync()
    {
        StringBuilder htmlBuilder = await ReadTemplateAsync();
        string graphContent = GenerateContent();
            
        var placeholders = new Dictionary<string, string>
        {
            { "{{Title}}", "Service Graph" },
            { "{{GraphContent}}",  graphContent},
        };

        foreach (KeyValuePair<string, string> placeholder in placeholders)
        {
            htmlBuilder.Replace(placeholder.Key, placeholder.Value);
        }
        
        return htmlBuilder.ToString();
    }

    private string GenerateContent()
    {
        GraphvizAlgorithm<Type, Edge<Type>> graphviz = _dependencyGraphBuilder.BuildGraph();
      
        // TODO: handle cycle dependencies
        // var cycleDetector = new CycleDetector(graphviz);
        // Tuple<Type, Type>? circularServices = cycleDetector.TryFindCircularDependentServices();

        return graphviz.Generate();
    }

    private async Task<StringBuilder> ReadTemplateAsync()
    {
        Assembly assembly = typeof(ServiceGraphUIMiddleware).GetTypeInfo().Assembly;

        await using Stream stream = assembly.GetManifestResourceStream(TemplateFileName);
        if (stream == null)
        {
            throw new FileNotFoundException("Template file not found: " + TemplateFileName);
        }

        using var reader = new StreamReader(stream);
        var template = new StringBuilder(await reader.ReadToEndAsync());
            
        return template;
    }
}