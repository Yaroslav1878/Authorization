using MediatR;

namespace Authorization.Application.Models.Commands.Token;

public class LogoutCommand : IRequest<Unit>
{
    public string RefreshToken { get; set; }
}
