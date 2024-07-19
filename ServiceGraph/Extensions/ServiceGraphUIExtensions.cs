using Microsoft.AspNetCore.Builder;
using ServiceGraph.Core;
using ServiceGraph.Visualization.Core;

namespace ServiceGraph.Extensions;

public static class ServiceGraphUIExtensions
{
    public static IApplicationBuilder UseServiceGraphUI(this IApplicationBuilder app,  ServiceGraphOption option)
    {
        return app.UseMiddleware<ServiceGraphUIMiddleware>();
    }
}