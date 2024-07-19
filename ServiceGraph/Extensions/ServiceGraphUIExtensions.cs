using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ServiceGraph.Core;
using ServiceGraph.Visualization.Core;

namespace ServiceGraph.Extensions;

public static class ServiceGraphUIExtensions
{
    public static IApplicationBuilder UseServiceGraphUI(this IApplicationBuilder app, IServiceCollection serviceCollection, ServiceGraphOption option)
    {
        return app.UseMiddleware<ServiceGraphUIMiddleware>(serviceCollection, option);
    }
}