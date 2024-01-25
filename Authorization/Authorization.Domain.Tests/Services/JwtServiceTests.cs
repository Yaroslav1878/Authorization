using System.Linq;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Models;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

#pragma warning disable CA2211, SA1401

namespace Authorization.Domain.Tests.Services
{
    public class JwtServiceTests
    {
        private readonly IJwtConfiguration _configuration;
        private readonly IUser _user;
        private readonly JwtService _sut;
        public JwtServiceTests()
        {
            _configuration = MockData.SetupDefaultJwtConfiguration();
            _user = MockData.SetupDefaultUser();
            var timeProvider = MockData.SetupDefaultTimeProvider();
            _sut = new JwtService(_configuration, timeProvider);
        }

        [Theory]
        [InlineData("issuer1")]
        [InlineData("issuer2")]
        public void CreateJwt_ChangingIssuerInConfiguration_SetsProperIssuer(string issuer)
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

            // Arrange
            _configuration.Issuer.Returns(issuer);

            // Act
            var jwt = _sut.CreateJwt(userDto);

            // Assert
            jwt.Issuer.Should().Be(issuer);
        }

        [Theory]
        [InlineData("authority1")]
        [InlineData("authority2")]
        public void CreateJwt_ChangingAuthorityInConfiguration_SetsProperAuthority(string authority)
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

            // Arrange
            _configuration.Authority.Returns(authority);

            // Act
            var jwt = _sut.CreateJwt(userDto);

            // Assert
            jwt.Audiences.Should().ContainSingle();
            jwt.Audiences.Single().Should().Be(authority);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(24)]
        public void CreateJwt_ChangingExpirationTimeInConfiguration_SetsProperValidTo(int expirationTimeInMinutes)
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

            // Arrange
            _configuration.ExpirationTimeInMinutes.Returns(expirationTimeInMinutes);

            // Act
            var jwt = _sut.CreateJwt(userDto);

            // Assert
            (jwt.ValidTo - jwt.ValidFrom).TotalMinutes.Should().Be(expirationTimeInMinutes);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(200)]
        public void CreateJwt_ChangingExpirationTimeInConfiguration_SetsValidFromSameAsIssuedAt(int expirationTimeInMinutes)
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

            _configuration.ExpirationTimeInMinutes.Returns(expirationTimeInMinutes);

            // Act
            var jwt = _sut.CreateJwt(userDto);

            // Assert
            jwt.ValidFrom.Should().Be(jwt.IssuedAt);
        }

        [Fact]
        public void ValidateToken_WithJWT_ShouldBeTrue()
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

            // Arrange
            var jwtString = _sut.GetJwtString(_sut.CreateJwt(userDto));

            // Act
            _sut.ValidateToken(jwtString);
        }
    }
}

#pragma warning restore CA2211, SA1401
