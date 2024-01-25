using System;

namespace Authorization.Domain.Services.Abstraction;

public interface ITimeProviderService
{
    DateTime UtcNow { get; }
}
