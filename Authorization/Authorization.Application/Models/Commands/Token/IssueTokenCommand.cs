using System;
using Authorization.Application.Models.Responses.Token;
using MediatR;

namespace Authorization.Application.Models.Commands.Token;

public class IssueTokenCommand : IRequest<TokenResponseModel>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public Guid ClientId { get; set; }
}
