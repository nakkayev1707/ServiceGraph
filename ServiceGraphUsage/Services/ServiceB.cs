using ServiceGraphUsage.Services.Abstract;

namespace ServiceGraphUsage.Services;

public class ServiceB : IServiceB, IServiceA
{
    private readonly IServiceC _serviceC;

    public ServiceB(IServiceC serviceC)
    {
        _serviceC = serviceC;
    }
}