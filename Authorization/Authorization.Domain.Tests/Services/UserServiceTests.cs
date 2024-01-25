using System;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Repositories.Abstractions;
using Authorization.Domain.Services;
using Authorization.Domain.Services.Abstraction;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Authorization.Domain.Tests.Services;

public class UserServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleService _roleService;
    private readonly IHashService _hashService;
    private readonly UserService _sut;

    public UserServiceTests(UserService sut)
    {
        _sut = sut;
        _userRepository = Substitute.For<IUserRepository>();
        _roleService = Substitute.For<IRoleService>();
        _hashService = Substitute.For<IHashService>();

        //_roleService.FindByName(Arg.Any<string>()).Returns(Task.FromResult(MockData.GetDefaultRole()));
        //_userRepository.FindByEmail(Arg.Any<string>()).Returns(Task.FromResult<User>(null));
        var defaultUser = MockData.GetDefaultUser().UserAuths.FirstOrDefault();
        if (defaultUser != null)
        {
            _hashService.CreateSalt().Returns(defaultUser.Salt);
            _hashService.CreateHash(Arg.Any<string>(), defaultUser.Salt!).Returns(defaultUser.PasswordHash);
        }

        //_sut = new UserService(_userRepository, _roleService, _hashService);
    }

    /* [Fact]
     public void RegisterUser_RoleNotFound_ThrowsRoleNotFoundException()
     {
         // Arrange
         _roleService.FindByName(Arg.Any<string>()).Returns(Task.FromResult<Role>(null));
         var (email, firstName, lastName) = GetRegistrationInput();

         // Act
         Func<Task> action = () => _sut.Create(email, firstName, lastName);

         // Assert
         action.Should().ThrowExactly<RoleNotFoundException>();
     }*/

    /*  [Fact]
      public void RegisterUser_UserWithSuchMailAlreadyExists_ThrowsDuplicateEmailException()
      {
          // Arrange
          _userRepository.Find(Arg.Any<string>()).Returns(Task.FromResult(MockData.GetDefaultUser()));
          var (email, password, firstName, lastName) = GetRegistrationInput();

          // Act
          Func<Task> action = () => _sut.RegisterUser(email, password, firstName, lastName);

          // Assert
          action.Should().ThrowExactly<DuplicateEmailException>();
      }*/

    [Fact]
    public async Task RegisterUser_WithDefaultValues_CreatesARandomSaltViaCallingCreateSalt()
    {
        // Arrange
        var (email, firstName, lastName) = GetRegistrationInput();

        // Act
        await _sut.Create(email, firstName, lastName);

        // Assert
        _hashService.Received(1).CreateSalt();
    }

    /*    [Fact]
        public void RegisterUser_WithDefaultValues_CallsHashGeneratorForPasswordHashing()
        {
            // Arrange
            var (email, firstName, lastName) = GetRegistrationInput();

            // Act
            _sut.Create(email, firstName, lastName);

            // Assert
            _hashService.Received(1).CreateHash(password, Arg.Any<string>());
        }*/

    /*   [Fact]
       public void RegisterUser_WithDefaultValues_CallsRepositoryInsert()
       {
           // Arrange
           var (email, firstName, lastName) = GetRegistrationInput();

           // Act
           _sut.Create(email, firstName, lastName);

           // Assert
           _userRepository.Received(1).Insert(Arg.Any<User>());
       }*/

    [Fact]
    public async Task RegisterUser_WithDefaultValues_ReturnsInactiveUser()
    {
        // Arrange
        var (email, firstName, lastName) = GetRegistrationInput();

        // Act
        var user = await _sut.Create(email, firstName, lastName);

        // Assert
        user.Active.Should().BeFalse();
    }

    [Fact]
    public async Task RegisterUser_WithDefaultValues_CorrectlyPopulatesOtherUserProperties()
    {
        // Arrange
        var (email, firstName, lastName) = GetRegistrationInput();

        // Act
        var user = await _sut.Create(email, firstName, lastName);

        // Assert
        user.Email.Should().Be(email);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Role.Should().NotBeNull();
    }

    [Fact]
    public void GetUser_WithWrongEmail_ThrowsWrongEmailUserException()
    {
        // Arrange
        var user = MockData.SetupDefaultUser();
        var email = user.Email;

        // Act
        Func<Task> action = async () => await _userRepository.FirstOrDefaultAsync(x => x.Email == email);

        // Assert
        action.Should().ThrowExactly<UserNotFoundException>();
    }

    [Fact]
    public void GetUser_WithWrongUserId_ThrowsWrongUserIdUserException()
    {
        // Arrange
        var user = MockData.SetupDefaultUser();
        var userId = user.Id;

        // Act
        Func<Task> action = async () => await _sut.GetById(userId);

        // Assert
        action.Should().ThrowExactly<UserNotFoundException>();
    }

    /*       [Fact]
           public void ChangePassword_WithDuplicatedPasswords_ThrowsDuplicatePasswordUserException()
           {
               // Arrange
               const string newPassword = "123456789";
               const int userId = 1;

               // Act
               var action = _sut.UpdatePassword(userId, newPassword);

               // Assert
               action.Should().ThrowExactly<PasswordChangeException>();
           }*/

    private static (string email, string firstName, string lastName) GetRegistrationInput()
    {
        var user = MockData.SetupDefaultUser();
        var email = user.Email;
        var firstName = user.FirstName;
        var lastName = user.LastName;

        return (email, firstName, lastName);
    }
}
