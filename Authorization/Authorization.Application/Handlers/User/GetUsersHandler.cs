using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.UserManagement;
using Authorization.Application.Models.Responses.User;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;
using MediatR;

namespace Authorization.Application.Handlers.User;

public class GetUsersHandler(
    IUserService userService,
    IMapper mapper) : IRequestHandler<GetUsersCommand, IReadOnlyCollection<UserResponseModel>>
{
    public async Task<IReadOnlyCollection<UserResponseModel>> Handle(
        GetUsersCommand request,
        CancellationToken cancellationToken)
    {
        var response = await userService.GetUsers(request.UserIds, request.RolesFilter);
        return mapper.Map<List<UserResponseModel>>(response);
    }
}
