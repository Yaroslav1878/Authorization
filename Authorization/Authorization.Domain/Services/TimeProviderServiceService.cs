using System;
using Authorization.Domain.Services.Abstraction;

namespace Authorization.Domain.Services;

public class TimeProviderServiceService : ITimeProviderService
{
    public DateTime UtcNow => DateTime.UtcNow;
}
