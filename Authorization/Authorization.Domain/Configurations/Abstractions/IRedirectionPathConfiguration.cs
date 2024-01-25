using System;

namespace Authorization.Domain.Configurations.Abstractions;

public interface IRedirectionPathConfiguration
{
    string Url { get; set; }

    string EmailActivationPath { get; set; }

    string PasswordRecoveryPath { get; set; }

    string LoginPath { get; set; }

    Uri LoginUrl { get; }
}
