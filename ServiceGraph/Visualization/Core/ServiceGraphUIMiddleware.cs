using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Spectre.Console;

namespace ServiceGraph.Visualization.Core
{
    public class ServiceGraphUIMiddleware
    {
        private const string RoutePrefix = "service-graph";
        private static bool _informed = false;
        private readonly RequestDelegate _next;

        public ServiceGraphUIMiddleware(RequestDelegate next)
        {
            _next = next;
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
                string htmlTemplate = await BuildHtmlTemplateAsync();
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

        private async Task<string> BuildHtmlTemplateAsync()
        {
            var htmlBuilder = new StringBuilder();

            htmlBuilder.Append("<!DOCTYPE html>");
            htmlBuilder.Append("<html lang=\"en\">");
            htmlBuilder.Append("<head>");
            htmlBuilder.Append("<meta charset=\"UTF-8\">");
            htmlBuilder.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            htmlBuilder.Append("<title>Service Graph</title>");
            htmlBuilder.Append("<style>");
            // Add any CSS styles here
            htmlBuilder.Append("body { font-family: Arial, sans-serif; }");
            htmlBuilder.Append("h1 { color: #333; }");
            htmlBuilder.Append("#chartContainer { width: 80%; margin: 0 auto; }");
            htmlBuilder.Append("</style>");
            htmlBuilder.Append("</head>");
            htmlBuilder.Append("<body>");
            htmlBuilder.Append("<h1>Service Graph</h1>");
            htmlBuilder.Append("<div id=\"chartContainer\">");
            htmlBuilder.Append("<canvas id=\"myChart\"></canvas>");
            htmlBuilder.Append("</div>");
            htmlBuilder.Append("<script src=\"https://cdn.jsdelivr.net/npm/chart.js\"></script>");
            htmlBuilder.Append("<script>");
            htmlBuilder.Append("var ctx = document.getElementById('myChart').getContext('2d');");
            htmlBuilder.Append("var myChart = new Chart(ctx, {");
            htmlBuilder.Append("type: 'bar',");
            htmlBuilder.Append("data: {");
            htmlBuilder.Append("labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],");
            htmlBuilder.Append("datasets: [{");
            htmlBuilder.Append("label: '# of Votes',");
            htmlBuilder.Append("data: [12, 19, 3, 5, 2, 3],");
            htmlBuilder.Append("backgroundColor: [");
            htmlBuilder.Append("'rgba(255, 99, 132, 0.2)',");
            htmlBuilder.Append("'rgba(54, 162, 235, 0.2)',");
            htmlBuilder.Append("'rgba(255, 206, 86, 0.2)',");
            htmlBuilder.Append("'rgba(75, 192, 192, 0.2)',");
            htmlBuilder.Append("'rgba(153, 102, 255, 0.2)',");
            htmlBuilder.Append("'rgba(255, 159, 64, 0.2)'");
            htmlBuilder.Append("],");
            htmlBuilder.Append("borderColor: [");
            htmlBuilder.Append("'rgba(255, 99, 132, 1)',");
            htmlBuilder.Append("'rgba(54, 162, 235, 1)',");
            htmlBuilder.Append("'rgba(255, 206, 86, 1)',");
            htmlBuilder.Append("'rgba(75, 192, 192, 1)',");
            htmlBuilder.Append("'rgba(153, 102, 255, 1)',");
            htmlBuilder.Append("'rgba(255, 159, 64, 1)'");
            htmlBuilder.Append("],");
            htmlBuilder.Append("borderWidth: 1");
            htmlBuilder.Append("}]");
            htmlBuilder.Append("},");
            htmlBuilder.Append("options: {");
            htmlBuilder.Append("scales: {");
            htmlBuilder.Append("y: {");
            htmlBuilder.Append("beginAtZero: true");
            htmlBuilder.Append("}");
            htmlBuilder.Append("}");
            htmlBuilder.Append("}");
            htmlBuilder.Append("});");
            htmlBuilder.Append("</script>");
            htmlBuilder.Append("</body>");
            htmlBuilder.Append("</html>");
            
            return htmlBuilder.ToString();
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