using System;
using Authorization.Domain.Configurations.Abstractions;

namespace Authorization.Domain.Configurations;

public class RedirectionPathConfiguration : IRedirectionPathConfiguration
{
    public string Url { get; set; }

    public string EmailActivationPath { get; set; }

    public string PasswordRecoveryPath { get; set; }

    public string LoginPath { get; set; }

    public Uri LoginUrl => new (Url + LoginPath);
}
