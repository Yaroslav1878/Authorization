using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.User;
using Authorization.Application.Models.Responses.User;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Services.Abstraction;
using AutoMapper;
using MediatR;

namespace Authorization.Application.Handlers.User
{
    public class DeleteOwnAvatarHandler(
        IUserService userService,
        IMapper mapper) : IRequestHandler<DeleteOwnAvatarCommand, UserResponseModel>
    {
        public async Task<UserResponseModel> Handle(DeleteOwnAvatarCommand request, CancellationToken cancellationToken)
        {
            UserDto userDto = await userService.DeleteAvatar(request.UserId);

            return mapper.Map<UserResponseModel>(userDto);
        }
    }
}