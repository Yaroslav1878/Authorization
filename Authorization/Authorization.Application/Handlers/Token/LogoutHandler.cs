using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.Token;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Repositories.Abstractions;
using Authorization.Domain.Services.Abstraction;
using MediatR;

namespace Authorization.Application.Handlers.Token;

public class LogoutHandler(
    ITokenService tokenService,
    IUnitOfWork unitOfWork) : IRequestHandler<LogoutCommand, Unit>
{
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        RefreshTokenDto? refreshToken = await tokenService.GetRefreshToken(request.RefreshToken);

        if (refreshToken != null)
        {
            await tokenService.LogoutRevokeRefreshTokens(refreshToken);
            await unitOfWork.Commit();
        }

        return Unit.Value;
    }
}