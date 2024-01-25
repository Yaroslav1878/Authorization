using Authorization.Application.Models.Responses.Token;
using MediatR;

namespace Authorization.Application.Models.Commands.Token;

public class RefreshTokenCommand : IRequest<TokenResponseModel>
{
    public string RefreshToken { get; set; }
}
