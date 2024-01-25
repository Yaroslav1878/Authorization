namespace Authorization.Application.Models.Responses.Token;

public class RefreshTokenResponseModel
{
    public string RefreshToken { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public bool Success { get; set; }
}
