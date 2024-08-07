﻿using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using QuickGraph;
using QuickGraph.Graphviz;
using ServiceGraph.Core;

namespace ServiceGraph.Visualization.Core;

public class HtmlBuilder
{
    private const string TemplateFileName = "ServiceGraph.Visualization.Core.service-graph.html";
    private readonly GraphvizAlgorithm<Type, Edge<Type>> _graphviz;
    
    public HtmlBuilder(ServiceGraphOption? graphOption, ServiceCollection serviceCollection)
    {
        var dependencyGraphBuilder = new DependencyGraphBuilder(serviceCollection, graphOption);
        _graphviz = dependencyGraphBuilder.BuildGraph();
    }
    
    public async Task<string> BuildAsync()
    {
        StringBuilder htmlBuilder = await ReadTemplateAsync();

        string graphContent = _graphviz.Generate();
        graphContent = graphContent.Replace("`", "\\`");
        
        var placeholders = new Dictionary<string, string>
        {
            { "{{Title}}", "Service Graph" },
            { "{{GraphContent}}",  graphContent},
            { "{{CircularDependency}}", CreateCircularDependencyMessage()}
        };

        foreach (KeyValuePair<string, string> placeholder in placeholders)
        {
            htmlBuilder.Replace(placeholder.Key, placeholder.Value);
        }
        
        return htmlBuilder.ToString();
    }

    private string CreateCircularDependencyMessage()
    {
        var stringBuilder = new StringBuilder();
        var cycleDetector = new CycleDetector(_graphviz);
        
        Tuple<Type, Type>? circularServices = cycleDetector.TryFindCircularDependentServices();
        if (circularServices != null)
        {
            stringBuilder.AppendLine($"cycle detected: {circularServices.Item1.FullName} => {circularServices.Item2.FullName}");
        }

        return stringBuilder.ToString();
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