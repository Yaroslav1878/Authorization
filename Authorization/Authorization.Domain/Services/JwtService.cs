using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Services.Abstraction;
using Microsoft.IdentityModel.Tokens;
using PemUtils;

namespace Authorization.Domain.Services;

public class JwtService(
        IJwtConfiguration jwtConfiguration,
        ITimeProviderService timeProviderService) : IJwtService
{
    public JwtSecurityToken CreateJwt(UserDto user)
    {
        var pathPrivateKey = Directory.GetCurrentDirectory() + "/private-key.pem";
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = CreateClaimsIdentity(user);
        var now = timeProviderService.UtcNow;

        RsaSecurityKey rsaKey;
        using (var stream = File.OpenRead(pathPrivateKey))
        using (var reader = new PemReader(stream))
        {
            rsaKey = new RsaSecurityKey(reader.ReadRsaKey());
        }

        var signingCredentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSsaPssSha256);
        var token = tokenHandler.CreateJwtSecurityToken(
            jwtConfiguration.Issuer,
            jwtConfiguration.Authority,
            claims,
            now,
            now.AddMinutes(jwtConfiguration.ExpirationTimeInMinutes),
            now,
            signingCredentials);

        return token;
    }

    public string GetJwtString(JwtSecurityToken token) => new JwtSecurityTokenHandler().WriteToken(token);

    public void ValidateToken(string token)
    {
        var pathPublicKey = Directory.GetCurrentDirectory() + "/public-key.pem";
        byte[] publicKey;
        using (var publicKeyFileStream = new StreamReader(pathPublicKey))
        {
            var publicKeyString = publicKeyFileStream.ReadToEnd()
                .Replace("-----BEGIN PUBLIC KEY-----", " ", StringComparison.CurrentCulture)
                .Replace("-----END PUBLIC KEY-----", " ", StringComparison.CurrentCulture);
            publicKey = Convert.FromBase64String(publicKeyString);
        }

        using RSA rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(publicKey, out _);

        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfiguration.Issuer,
                ValidAudience = jwtConfiguration.Authority,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                CryptoProviderFactory = new CryptoProviderFactory()
                {
                    CacheSignatureProviders = false,
                },
            },
            out _);
    }

    private static ClaimsIdentity CreateClaimsIdentity(UserDto user)
    {
        var claimsIdentity = new ClaimsIdentity();

        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.FirstName));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Role.Id));

        var roleScopes = user.Role.RoleScopes.DistinctBy(x => x.ScopeId);
        claimsIdentity.AddClaims(roleScopes.Select(roleScope => new Claim("scopes", roleScope.ScopeId)));

        return claimsIdentity;
    }
}
