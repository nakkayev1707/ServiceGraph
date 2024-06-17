using ServiceGraphUsage.Services.Abstract;

namespace ServiceGraphUsage.Services;

public class ServiceA : IServiceA
{
    private readonly IServiceB _serviceB;

    public ServiceA(IServiceB serviceB)
    {
        _serviceB = serviceB;
    }
}