using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ServiceGraph.Core;
using Spectre.Console;

namespace ServiceGraph.Visualization.Core
{
    public class ServiceGraphUIMiddleware
    {
        private const string RoutePrefix = "service-graph";
        private static bool _informed;
        private readonly RequestDelegate _next;
        private readonly ServiceGraphOption _graphOption;
        private readonly ServiceCollection _serviceCollection;

        public ServiceGraphUIMiddleware(RequestDelegate next, ServiceCollection serviceCollection, ServiceGraphOption graphOption)
        {
            _next = next;
            _graphOption = graphOption;
            _serviceCollection = serviceCollection;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string? httpMethod = httpContext.Request.Method;
            string? path = httpContext.Request.Path.Value;

            PrintServiceGraphUILaunchUrl(httpContext);

            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/?{RoutePrefix}/?$", RegexOptions.IgnoreCase))
            {
                RespondWithRedirect(httpContext.Response, $"{RoutePrefix}/index.html");
                return;
            }

            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{RoutePrefix}/index.html$", RegexOptions.IgnoreCase))
            {
                await RespondWithIndexHtml(httpContext.Response);
                return;
            }

            await _next(httpContext);
        }

        private async Task RespondWithIndexHtml(HttpResponse response)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html;charset=utf-8";

            try
            {
                string htmlTemplate = await new HtmlBuilder(_graphOption, _serviceCollection).BuildAsync();
                await response.WriteAsync(htmlTemplate, Encoding.UTF8);
            }
            catch (FileNotFoundException)
            {
                response.StatusCode = 404;
                await response.WriteAsync("File not found.");
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                await response.WriteAsync($"Internal server error: {ex.Message}");
            }
        }

        private void RespondWithRedirect(HttpResponse response, string location)
        {
            response.StatusCode = 301;
            response.Headers["Location"] = location;
        }

        private static void PrintServiceGraphUILaunchUrl(HttpContext httpContext)
        {
            if (_informed) return;

            _informed = true;
            var request = httpContext.Request;
            var host = request.Host.Value;
            var scheme = request.Scheme;
            var url = $"{scheme}://{host}/{RoutePrefix}/index.html";

            AnsiConsole.MarkupLine($"[bold green]ServiceGraphUI: {url} [/]");
        }
    }
}