using System.IdentityModel.Tokens.Jwt;
using Authorization.Domain.Models.Dtos;

namespace Authorization.Domain.Services.Abstraction;

public interface IJwtService
{
    JwtSecurityToken CreateJwt(UserDto user);
    string GetJwtString(JwtSecurityToken token);
    void ValidateToken(string token);
}
