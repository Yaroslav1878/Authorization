using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authorization.Application.Models.Commands.Confirmation;
using Authorization.Domain.Enums;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Models.Dtos;
using Authorization.Domain.Repositories.Abstractions;
using Authorization.Domain.Services.Abstraction;
using MediatR;

namespace Authorization.Application.Handlers.Registration;

public class ConfirmResendHandler(
    IRegistrationService registrationService,
    IUserService userService,
    IUnitOfWork unitOfWork) : IRequestHandler<ConfirmResendCommand, Unit>
{
    public async Task<Unit> Handle(ConfirmResendCommand request, CancellationToken cancellationToken)
    {
        UserDto userDto = await userService.GetById(request.UserId);

        if (userDto.Status != UserStatus.Invited)
        {
            throw new BadRequestException(ErrorCode.UserIsAlreadyActivated, $"User is {userDto.Status}");
        }

        if (!userDto.UserAuths.Any())
        {
            await registrationService.InviteUser(request.UserId, userDto.Email, userDto.FirstName);
        }

        await unitOfWork.Commit();
        return Unit.Value;
    }
}
