using Microsoft.Extensions.DependencyInjection;

namespace ServiceGraph.Core;

public class ServiceDescriptorScanner
{
    private readonly IServiceCollection _serviceCollection;

    public ServiceDescriptorScanner(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public IEnumerable<ServiceDescriptor> Scan()
    {
        return _serviceCollection;
    }
}