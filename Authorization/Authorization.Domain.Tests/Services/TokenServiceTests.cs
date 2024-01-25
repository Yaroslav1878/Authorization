using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Enums;
using Authorization.Domain.Models;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Repositories.Abstractions;
using Authorization.Domain.Services;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;
using NSubstitute;
using Xunit;

namespace Authorization.Domain.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtService _jwtService;
        private readonly IUser _user;
        private readonly TokenService _sut;

        public TokenServiceTests()
        {
            _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
            var configuration = Substitute.For<ITokensConfiguration>();
            _jwtService = Substitute.For<IJwtService>();
            var mapper = Substitute.For<IMapper>();
            var unitOfWork = Substitute.For<IUnitOfWork>();
            _user = MockData.SetupDefaultUser();
            var timeProvider = MockData.SetupDefaultTimeProvider();

            _sut = new TokenService(_refreshTokenRepository, configuration, timeProvider, _jwtService, mapper, unitOfWork);
        }

        [Fact]
        public void IssueAccessToken_WithDefaultValues_CallsCreateJwt()
        {
            // Arrange
            UserDto userDto = new ()
            {
                Id = _user.Id,
                Email = _user.Email,
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                RoleId = _user.RoleId,
                Role = _user.Role
            };

            // Act
            _sut.IssueAccessToken(userDto);

            // Assert
            _jwtService.Received(1).CreateJwt(userDto);
        }

        [Fact]
        public void IssueAccessToken_WithDefaultValues_CallsGetJwtString()
        {
            // Arrange
            UserDto userDto = new ()
            {
                Id = _user.Id,
                Email = _user.Email,
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                RoleId = _user.RoleId,
                Role = _user.Role
            };

            // Act
            _sut.IssueAccessToken(userDto);

            // Assert
            _jwtService.Received(1).GetJwtString(Arg.Any<JwtSecurityToken>());
        }

        [Fact]
        public async Task IssueRefreshToken_WithDefaultValues_CallsRevokeRefreshTokens()
        {
            // Arrange
            UserDto userDto = new ()
            {
                Id = _user.Id,
                Email = _user.Email,
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                RoleId = _user.RoleId,
                Role = _user.Role
            };
            ClientDto client = new() 
            {
                Id = new Guid("aaaa1111-0000-0000-0000-000000000001"),
            };

            // Act
            await _sut.IssueRefreshToken(userDto, client.Id);

            // Assert
            await _refreshTokenRepository.Received(1).RevokeRefreshTokens(userDto.Id, client.Id, RefreshTokenRevokeReason.Refresh);
        }

      /*  [Fact]
        public async Task IssueRefreshToken_WithDefaultValues_CallsRepositoryInsert()
        {
            // Arrange
            var user = new UserDto();
            var client = new ClientDto();

            // Act
            await _sut.IssueRefreshToken(user, client.Id);

            // Assert
            await _refreshTokenRepository.Received(1).Insert(Arg.Any<RefreshToken>());
        }

        [Fact]
        public async Task IssueRefreshToken_WithDefaultValues_SetsProperUserAndClientPropertiesInRefreshToken()
        {
            // Arrange
            UserDto userDto = new ()
            {
                Id = _user.Id,
                Email = _user.Email,
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                RoleId = _user.RoleId,
                Role = _user.Role
            };
            ClientDto client = new() 
            {
                Id = new Guid("aaaa1111-0000-0000-0000-000000000001"),
            };

            // Act
            var refreshToken = await _sut.IssueRefreshToken(userDto, client.Id);

            // Assert
            refreshToken.User.Should().Be(userDto);
            refreshToken.Client.Should().Be(client);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(200)]
        public async Task IssueRefreshToken_WithDefaultValues_SetsProperExpireAt(int expirationTimeInMinutes)
        {
            // Arrange
            var user = new UserDto();
            var client = new ClientDto();
            _configuration.RefreshTokenExpirationTimeInHours.Returns(expirationTimeInMinutes);

            // Act
            var refreshToken = await _sut.IssueRefreshToken(user, client.Id);

            // Assert
            (refreshToken.ExpireAt - refreshToken.CreatedAt).TotalHours.Should().Be(expirationTimeInHours);
        }*/
    }
}
