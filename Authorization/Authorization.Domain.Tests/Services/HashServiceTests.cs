using System;
using Authorization.Domain.Services;
using FluentAssertions;
using Xunit;

namespace Authorization.Domain.Tests.Services
{
    public class HashServiceTests
    {
        private const int SaltLength = 128 / 8;
        private readonly HashService _sut = new();

        [Fact]
        public void CreateSalt_WithDefaultValues_GeneratesNonEmptySalt()
        {
            // Act
            var salt = _sut.CreateSalt();

            // Assert
            salt.Should().NotBeEmpty();
        }

        [Fact]
        public void CreateSalt_WithDefaultValues_GeneratesBase64String()
        {
            // Arrange
            var saltBytes = new byte[SaltLength];

            // Act
            var salt = _sut.CreateSalt();

            // Assert
            var isBase64String = Convert.TryFromBase64String(salt, saltBytes, out _);
            isBase64String.Should().BeTrue();
        }

        [Fact]
        public void CreateSalt_WithDefaultValues_GeneratesStringWithFixedLength()
        {
            // Act
            var salt = _sut.CreateSalt();

            // Assert
            salt.Length.Should().Be(24);
        }

        [Theory]
        [InlineData("qwerty123", "zmfk9Uxd6ggMHUNl16CnzQ==", "86cAAIGALPG4sOg8MvANc1TnHlIPH8Re8UQeh8+EEFM=")]
        [InlineData("changeme", "zmfk9Uxd6ggMHUNl16CnzQ==", "6b3iZOJ5dHHn6rkJPjf6eOqbpfzbCdEeUw7hJy/VyKY=")]
        [InlineData("qwerty123", "ytEv2UkDqkfSa8Y3zsQjQg==", "liIAi2kSVsNsHKdFeNvwYw2tt7NTAwemsnpWIFboyNA=")]
        public void GenerateHash_ForValidPasswordAndSalt_GeneratesAValidHash(string password, string salt, string expectedHash)
        {
            // Act
            var passwordHash = _sut.CreateHash(password, salt);

            // Assert
            passwordHash.Should().Be(expectedHash);
        }
    }
}
