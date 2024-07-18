using System.Reflection;
using System.Text;

namespace ServiceGraph.Visualization.Core;

public class HtmlBuilder
{
    public async Task<string> BuildAsync()
    {
        StringBuilder htmlBuilder = await ReadTemplateAsync();
            
        var placeholders = new Dictionary<string, string>
        {
            { "{{Title}}", "My Service Graph" },
            { "{{Date}}", DateTime.Now.ToString("yyyy-MM-dd") },
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
        const string resourceName = "ServiceGraph.Visualization.Core.service-graph.html";

        await using Stream stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new FileNotFoundException("Resource not found: " + resourceName);
        }

        using var reader = new StreamReader(stream);
        var template = new StringBuilder(await reader.ReadToEndAsync());
            
        return template;
    }
}