﻿using ServiceGraphUsage.Services.Abstract;

namespace ServiceGraphUsage.Services;

public class ServiceA : IServiceA, IServiceB
{
    private readonly IServiceB _serviceB;

    public ServiceA(IServiceB serviceB)
    {
        _serviceB = serviceB;
    }
}