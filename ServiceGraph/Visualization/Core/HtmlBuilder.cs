using System.Reflection;
using System.Text;

namespace ServiceGraph.Visualization.Core;

public class HtmlBuilder
{
    private const string TemplateFileName = "ServiceGraph.Visualization.Core.service-graph.html";
    
    public async Task<string> BuildAsync()
    {
        StringBuilder htmlBuilder = await ReadTemplateAsync();
            
        var placeholders = new Dictionary<string, string>
        {
            { "{{Title}}", "Service Graph" },
            // { "{{GraphContent}}",  },
        };

        foreach (KeyValuePair<string, string> placeholder in placeholders)
        {
            htmlBuilder.Replace(placeholder.Key, placeholder.Value);
        }
        
        return htmlBuilder.ToString();
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