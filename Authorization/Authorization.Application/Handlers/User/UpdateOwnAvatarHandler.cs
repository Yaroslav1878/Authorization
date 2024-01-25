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
    public class UpdateOwnAvatarHandler(
        IUserService userService,
        IMapper mapper) : IRequestHandler<UpdateOwnAvatarCommand, UserResponseModel>
    {
        public async Task<UserResponseModel> Handle(UpdateOwnAvatarCommand request, CancellationToken cancellationToken)
        {
            UserDto userDto = await userService.UpdateAvatar(request.UserId, request.AvatarId);

            return mapper.Map<UserResponseModel>(userDto);
        }
    }
}