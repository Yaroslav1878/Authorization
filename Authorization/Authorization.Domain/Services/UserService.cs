using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Constants;
using Authorization.Domain.Enums;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Models.Entities;
using Authorization.Domain.Repositories.Abstractions;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Authorization.Domain.Services;

public class UserService(IUserRepository userRepository,
        IHashService hashService,
        ITokenService tokenService,
        IUserAuthRepository userAuthRepository,
        IUnitOfWork unitOfWork,
        IConfirmationRequestRepository confirmationRequestRepository,
        ITimeProviderService timeProviderService,
        IMapper mapper,
        IConfirmationRequestService confirmationRequestService,
        IAccountPolicyConfiguration accountPolicyConfiguration)
    : IUserService
{
    public async Task<UserDto> Create(string email, string firstName, string lastName)
    {
        var foundUsers = await userRepository.Find(user => user.Email == email);
        if (foundUsers != null && foundUsers.Any(user => user.Status != UserStatus.Deleted))
        {
            throw new DuplicateEmailException(email);
        }

        var user = new User
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Status = UserStatus.Invited,
            RoleId = Roles.User
        };

        EntityEntry<User> result = await userRepository.InsertAsync(user);

        await unitOfWork.Commit();

        return mapper.Map<UserDto>(result.Entity);
    }

    public async Task<UserAuth> CreateUserAuth(int userId, AuthType authType, string? password = null)
    {
        UserAuth? userAuth = null;

        if (authType == AuthType.Email && password != null)
        {
            var salt = hashService.CreateSalt();
            var passwordHash = hashService.CreateHash(password, salt);
            userAuth = new UserAuth
            {
                UserId = userId,
                PasswordHash = passwordHash,
                Salt = salt,
                AuthType = AuthType.Email,
                Subject = Subject.Password,
            };
        }

        if (userAuth != null)
        {
            EntityEntry<UserAuth> result = await userAuthRepository.InsertAsync(userAuth);

            await unitOfWork.Commit();

            return result.Entity;
        }

        throw new NotImplementedException();
    }

    public UserDto GetUserByEmailAndPassword(string email, string password)
    {
        User? user = userRepository.FirstOrDefault(
            user => user.Email == email && user.Status != UserStatus.Deleted
                                        && user.UserAuths.Any(userAuth => userAuth.AuthType == AuthType.Email),
            users => users.Include(user => user.Role)
                .ThenInclude(role => role.RoleScopes)
                .Include(user => user.UserAuths));

        if (user == null)
        {
            throw new AuthTypeNotFoundException(AuthType.Email);
        }

        var userAuth = user.UserAuths.First(userAuth => userAuth.AuthType == AuthType.Email);

        if (userAuth.Salt == null || userAuth.PasswordHash == null
                                  || !hashService.VerifyPassword(password, userAuth.Salt, userAuth.PasswordHash))
        {
            throw new InvalidCredentialException();
        }

        return mapper.Map<UserDto>(user);
    }
    
    public async Task<IReadOnlyCollection<UserDto>> GetUsers(
        IReadOnlyCollection<int> userIdsFilter,
        IReadOnlyCollection<string> rolesFilter)
    {
        var users = await userRepository.Find(
            user =>
                !userIdsFilter.Any() || userIdsFilter.Contains(user.Id),
            users => users.Include(user => user.Role)
                .ThenInclude(role => role.RoleScopes).ThenInclude(roleScope => roleScope.Scope)
                .Include(user => user.UserAuths));

        if (rolesFilter.Any())
        {
            users = users.Where(user => rolesFilter.Contains(user.Role.Id)).ToList();
        }

        var sortedUsers = users.OrderBy(user => user.Email);
        return mapper.Map<List<UserDto>>(sortedUsers);
    }

    public Task<UserDto> GetById(int userId)
    {
        var user = userRepository.FindFirst(user => user.Id == userId,
             user => user.UserAuths);
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        return Task.FromResult(mapper.Map<UserDto>(user));
    }

    public bool HasRole(int userId, string roleId)
    {
        return userRepository.FindFirst(user => user.Id == userId, user => user.Role).Role.Id == roleId;
    }

    public async Task<UserDto> UpdateUser(int userId, string firstName, string lastName)
    {
        var user = userRepository.FindFirst(user => user.Id == userId);
        if (user == null || user.Status == UserStatus.Deleted)
        {
            throw new UserNotFoundException(userId);
        }

        user.FirstName = firstName;
        user.LastName = lastName;

        userRepository.Update(user);
        await unitOfWork.Commit();

        return mapper.Map<UserDto>(user);
    }

    public void UpdatePassword(int userId, string password)
    {
        User user = userRepository.FindFirst(user => user.Id == userId, user => user.UserAuths);

        if (user == null || !user.Active)
        {
            throw new UserNotFoundException(userId);
        }

        var userAuth = user.UserAuths.FirstOrDefault(x => x.User.Id == userId && x.AuthType == AuthType.Email);

        if (userAuth == null)
        {
            throw new AuthTypeNotFoundException(AuthType.Email);
        }

        var salt = hashService.CreateSalt();
        var passwordHash = hashService.CreateHash(password, salt);
        userAuth.PasswordHash = passwordHash;
        userAuth.Salt = salt;

        userAuthRepository.Update(userAuth);
    }

    public async Task<UserDto> UpdateAvatar(int userId, Guid avatarId)
    {
        var user = userRepository.FindFirst(user => user.Id == userId);
        if (user == null || user.Status == UserStatus.Deleted)
        {
            throw new UserNotFoundException(userId);
        }

        user.AvatarId = avatarId;

        userRepository.Update(user);
        await unitOfWork.Commit();

        return mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> DeleteAvatar(int userId)
    {
        var user = userRepository.FindFirst(user => user.Id == userId);
        if (user == null || user.Status == UserStatus.Deleted)
        {
            throw new UserNotFoundException(userId);
        }

        if (user.AvatarId.HasValue)
        {
            user.AvatarId = null;

            userRepository.Update(user);
            await unitOfWork.Commit();
        }

        return mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateStatus(int userId, UserStatus status)
    {
        var user = userRepository.FindFirst(x => x.Id == userId);

        if (user == null || user.Status == UserStatus.Deleted)
        {
            throw new UserNotFoundException(userId);
        }

        user.Status = status;
        userRepository.Update(user);

        await tokenService.LogoutRevokeRefreshTokensByUserIds(new[] { userId });
        await unitOfWork.Commit();

        return mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> Delete(int userId)
    {
        var user = userRepository.FindFirst(x => x.Id == userId);

        if (user == null || user.Status == UserStatus.Deleted)
        {
            throw new UserNotFoundException(userId);
        }

        user.Status = UserStatus.Deleted;
        user.RemovedAt = DateTime.UtcNow;
        userRepository.Update(user);
        await unitOfWork.Commit();
        return mapper.Map<UserDto>(user);
    }

    public async Task<IReadOnlyCollection<AuthType>> GetUserAuthTypes(string email)
    {
        List<UserAuth> userAuths =
            await userAuthRepository.Find(userAuth => userAuth.User.Email == email && userAuth.User.Status != UserStatus.Deactivated,
                userAuth => userAuth.User);

        return userAuths.Select(x => x.AuthType).ToList();
    }

    public async Task SendPasswordResetEmail(string email)
    {
        var user = userRepository.FindFirst(user =>
            user.Email == email && user.Status == UserStatus.Active);
        
        await confirmationRequestService.Revoke(user.Id, ConfirmationRequestSubject.PasswordRecovery);

        ConfirmationRequest confirmationRequest = new ConfirmationRequest
        {
            Id = Guid.NewGuid(),
            CreatedAt = timeProviderService.UtcNow,
            ExpiredAt = timeProviderService.UtcNow.AddHours(accountPolicyConfiguration.PasswordRecoveryTimeout),
            UserId = user.Id,
            Subject = ConfirmationRequestSubject.PasswordRecovery,
            ConfirmationType = ConfirmationRequestType.Email,
            Receiver = email,
        };
        await confirmationRequestRepository.InsertAsync(confirmationRequest);
        await unitOfWork.Commit();
        
        var urlQuery = confirmationRequestService.GetUrlQuery(confirmationRequest.Id);

        // todo: add hangfire
        /*BackgroundJob.Enqueue(() => _emailService.SendPasswordRecovery(email, firstName, urlQuery.ToString()!));*/
    }

    public async Task ResetPassword(Guid confirmationId, string password)
    {
        var confirmationRequest = confirmationRequestService.Get(confirmationId);

        await UpdateUserAuth(confirmationRequest!.User.Id, AuthType.Email, password);

        await confirmationRequestService.Confirm(confirmationRequest.User.Id, ConfirmationRequestSubject.Registration);
        await unitOfWork.Commit();
    }

    public async Task<UserDto> UpdateUserRole(int userId, string roleId)
    {
        var user = userRepository.FindFirst(user => user.Id == userId, user => user.Role);
        user.RoleId = roleId;
        userRepository.Update(user);
        await unitOfWork.Commit();
        return mapper.Map<UserDto>(user);
    }

    private async Task UpdateUserAuth(int userId, AuthType authType, string password)
    {
        UserAuth? userAuth = null;
        if (authType == AuthType.Email)
        {
            userAuth =
                userAuthRepository.FindFirst(x => x.UserId == userId && x.AuthType == authType);
            var salt = hashService.CreateSalt();
            userAuth.PasswordHash = hashService.CreateHash(password, salt);
            userAuth.Salt = salt;
            userAuth.UpdatedAt = DateTime.UtcNow;
        }

        if (userAuth != null)
        {
            userAuthRepository.Update(userAuth);
            await unitOfWork.Commit();
        }
    }
}
