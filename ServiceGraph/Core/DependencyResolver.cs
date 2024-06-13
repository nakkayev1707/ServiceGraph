using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ServiceGraph.Graph;

namespace ServiceGraph.Core;

internal class DependencyResolver
{
    private ServiceGraphOption? _graphOption;

    public void ApplyOptions(ServiceGraphOption? option)
    {
        _graphOption = option;
    }
    
    public void AddResolver(IServiceCollection services)
    {
        Dictionary<Type, List<Type>> servicesDict = Resolve(services);

        var dependencyGraphBuilder = new DependencyGraphBuilder();
        dependencyGraphBuilder.BuildGraph(servicesDict);
    }
    
    private Dictionary<Type, List<Type>> Resolve(IEnumerable<ServiceDescriptor> serviceDescriptors)
    {
        var dependencies = new Dictionary<Type, List<Type>>();
        foreach (ServiceDescriptor serviceDescriptor in serviceDescriptors)
        {
            Type serviceType = serviceDescriptor.ServiceType;
            Type? implementationType = serviceDescriptor.ImplementationType;

            if (implementationType != null)
            {
                ConstructorInfo? ctor = implementationType.GetConstructors().FirstOrDefault();
                if (ctor != null)
                {
                    List<Type> parameterTypes = ctor.GetParameters().Select(p => p.ParameterType).ToList();
                    dependencies[serviceType] = parameterTypes;
                }
            }
        }

        return dependencies;
    }
}