using Microsoft.AspNetCore.Builder;
using ServiceGraph.Visualization.Core;

namespace ServiceGraph.Extensions;

public static class ServiceGraphUIExtensions
{
    private const string RoutePrefix = "service-graph";
    
    public static IApplicationBuilder UseServiceGraphUI(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<ServiceGraphUIMiddleware>();
    }
}