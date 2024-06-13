using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceGraph.Core;

internal class DependencyResolver
{
    public Dictionary<Type, List<Type>> Resolve(IEnumerable<ServiceDescriptor> serviceDescriptors)
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