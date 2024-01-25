using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authorization.Domain.Enums;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Models.Entities;

namespace Authorization.Domain.Services.Abstraction;

public interface IUserService
{
    Task<UserDto> Create(string email, string firstName, string lastName);
    Task<UserAuth> CreateUserAuth(int userId, AuthType authType, string? password = null);
    UserDto GetUserByEmailAndPassword(string email, string password);
    Task<IReadOnlyCollection<UserDto>> GetUsers(
        IReadOnlyCollection<int> userIdsFilter,
        IReadOnlyCollection<string> rolesFilter);
    Task<UserDto> GetById(int userId);
    bool HasRole(int userId, string roleId);
    Task<UserDto> UpdateUser(int userId, string firstName, string lastName);
    void UpdatePassword(int userId, string password);
    Task<UserDto> UpdateAvatar(int userId, Guid avatarId);
    Task<UserDto> DeleteAvatar(int userId);
    Task<UserDto> UpdateStatus(int userId, UserStatus status);
    Task<UserDto> Delete(int userId);
    Task<IReadOnlyCollection<AuthType>> GetUserAuthTypes(string email);
    Task SendPasswordResetEmail(string email);
    Task ResetPassword(Guid confirmationId, string password);
}
