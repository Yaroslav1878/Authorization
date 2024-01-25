using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.Token;
using Authorization.Application.Models.Responses.Token;
using Authorization.Application.Models.Responses.User;
using Authorization.Domain.Configurations.Abstractions;
using Authorization.Domain.Repositories.Abstractions;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;
using MediatR;

namespace Authorization.Application.Handlers.Token;

public class IssueTokenHandler(
        IUserService userService,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ITokensConfiguration tokensConfiguration) : IRequestHandler<IssueTokenCommand, TokenResponseModel>
{
    public async Task<TokenResponseModel> Handle(IssueTokenCommand request, CancellationToken cancellationToken)
    {
        var userDto = userService.GetUserByEmailAndPassword(request.Email, request.Password);
        var accessToken = tokenService.IssueAccessToken(userDto);
        var refreshToken = await tokenService.IssueRefreshToken(userDto, request.ClientId);

        await unitOfWork.Commit();

        UserResponseModel user = mapper.Map<UserResponseModel>(userDto);

        var response = new TokenResponseModel(accessToken, refreshToken, user, tokensConfiguration);

        return response;
    }
}
