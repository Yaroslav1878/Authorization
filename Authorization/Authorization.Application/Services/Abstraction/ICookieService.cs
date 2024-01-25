namespace Authorization.Application.Services.Abstraction;

public interface ICookieService
{
    public void SetAccessTokenCookie(string cookieNames, string value);
}