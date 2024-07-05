using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace ServiceGraph.Visualization.Core;

public class ServiceGraphUIMiddleware
{
    public async Task Invoke(HttpContext httpContext)
    {
        var httpMethod = httpContext.Request.Method;
        var path = httpContext.Request.Path.Value;

        if (httpMethod == "GET" && Regex.IsMatch(path, $"^/?{Regex.Escape(_options.RoutePrefix)}/?$",  RegexOptions.IgnoreCase))
        {
            // Use relative redirect to support proxy environments
            var relativeIndexUrl = string.IsNullOrEmpty(path) || path.EndsWith("/")
                ? "index.html"
                : $"{path.Split('/').Last()}/index.html";

            RespondWithRedirect(httpContext.Response, relativeIndexUrl);
            return;
        }

        if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{Regex.Escape(_options.RoutePrefix)}/?index.html$",  RegexOptions.IgnoreCase))
        {
            await RespondWithIndexHtml(httpContext.Response);
            return;
        }

        await _staticFileMiddleware.Invoke(httpContext);
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
}