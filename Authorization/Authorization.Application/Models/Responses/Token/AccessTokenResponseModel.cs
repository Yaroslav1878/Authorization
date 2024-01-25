using System;
using Authorization.Application.Models.Responses.User;

namespace Authorization.Application.Models.Responses.Token;

public class AccessTokenResponseModel
{
    public string AccessToken { get; set; }

    public string TokenType { get; set; }

    public UserResponseModel UserDetails { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime RefreshTokenExpiredAt { get; set; }

    public DateTime AccessTokenExpiredAt { get; set; }
}