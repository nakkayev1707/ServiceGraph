using Microsoft.Extensions.DependencyInjection;

namespace ServiceGraph.Core;

public class DependencyResolver
{
    public Dictionary<Type, List<Type>> Resolve(IEnumerable<ServiceDescriptor> serviceDescriptors)
    {
        
    }
}