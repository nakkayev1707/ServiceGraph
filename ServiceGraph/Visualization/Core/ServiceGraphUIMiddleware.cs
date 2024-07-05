using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace ServiceGraph.Visualization.Core;

public class ServiceGraphUIMiddleware
{
    private const string routePrefix = "service-graph";
    
    public async Task Invoke(HttpContext httpContext)
    {
        string? httpMethod = httpContext.Request.Method;
        string? path = httpContext.Request.Path.Value;

        if (httpMethod == "GET" && Regex.IsMatch(path, $"^/?{routePrefix}/?$",  RegexOptions.IgnoreCase))
        {
            string relativeIndexUrl = string.IsNullOrEmpty(path) || path.EndsWith("/")
                ? "index.html"
                : $"{path.Split('/').Last()}/index.html";

            RespondWithRedirect(httpContext.Response, relativeIndexUrl);
            return;
        }

        if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{routePrefix}/?index.html$",  RegexOptions.IgnoreCase))
        {
            await RespondWithIndexHtml(httpContext.Response);
        }
    }
    
    private async Task RespondWithIndexHtml(HttpResponse response)
    {
        response.StatusCode = 200;
        response.ContentType = "text/html;charset=utf-8";

        using (var stream = _options.IndexStream())
        {
            using var reader = new StreamReader(stream);

            // Inject arguments before writing to response
            var htmlBuilder = new StringBuilder(await reader.ReadToEndAsync());
            foreach (var entry in GetIndexArguments())
            {
                htmlBuilder.Replace(entry.Key, entry.Value);
            }

            await response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
        }
    }
    
    private void RespondWithRedirect(HttpResponse response, string location)
    {
        response.StatusCode = 301;
        response.Headers["Location"] = location;
    }
}