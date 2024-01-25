using System;
using System.Collections.Generic;
using System.Linq;
using Authorization.Application.Constants;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Constants;
using Authorization.Domain.Enums;
using Authorization.Domain.Models;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Models.Entities;
using Authorization.Domain.Services.Abstraction;
using NSubstitute;

namespace Authorization.Domain.Tests;

public static class MockData
{
    public static IJwtConfiguration SetupDefaultJwtConfiguration()
    {
        var configuration = Substitute.For<IJwtConfiguration>();

        configuration.Issuer.Returns("test-issuer");
        configuration.Authority.Returns("test-authority");
        configuration.ExpirationTimeInMinutes.Returns(1);

        return configuration;
    }

    public static IUser SetupDefaultUser()
    {
        var user = GetDefaultUser();
        var userProxy = Substitute.For<IUser>();

        UserAuth userAuth = new ()
        {
            Id = user.UserAuths.FirstOrDefault()!.Id,
            PasswordHash = user.UserAuths.FirstOrDefault()?.PasswordHash,
            Salt = user.UserAuths.FirstOrDefault()?.Salt,
            Subject = user.UserAuths.FirstOrDefault()!.Subject,
            CreatedAt = user.UserAuths.FirstOrDefault()!.CreatedAt,
            AuthType = user.UserAuths.FirstOrDefault()!.AuthType
        };

        userProxy.Id.Returns(user.Id);
        userProxy.Email.Returns(user.Email);
        userProxy.FirstName.Returns(user.FirstName);
        userProxy.LastName.Returns(user.LastName);
        userProxy.Active.Returns(user.Active);
        userProxy.Role.Returns(user.Role);
        userProxy.RoleId.Returns(user.RoleId);

        userProxy.UserAuths.Add(userAuth);

        return userProxy;
    }

    public static ITimeProviderService SetupDefaultTimeProvider()
    {
        var timeProvider = Substitute.For<ITimeProviderService>();

        timeProvider.UtcNow.Returns(DateTime.Now);
        return timeProvider;
    }

    public static Role GetDefaultRole()
    {
        return new Role { Id = "user" };
    }

    public static UserDto GetDefaultUser()
    {
        return new UserDto
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Smith",
            Id = 1,
            Status = UserStatus.Active,
            RoleId = Roles.User,
            Role = new ()
            {
                Id = Roles.User, RoleScopes = new[]
                {
                    new RoleScopeDto
                    {
                        ScopeId = Scopes.ViewUser,
                        RoleId = Roles.User
                    }
                }
            },
            UserAuths = new List<UserAuth>
            {
                new ()
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    AuthType = AuthType.Email,
                    Subject = Subject.Password,
                    PasswordHash = "bgu2Enqx/2rEk7awO30qy6iWS5U4JC4M0fsFM1EwV9E=",
                    Salt = "wbD2sRTiMhDfaestjRAA0A==",
                }
            }
        };
    }
}
