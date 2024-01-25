using Authorization.Domain.Enums;

namespace Authorization.Application.Models.Requests.User;

public class UpdateUserStatusRequestModel
{
    public UserStatus Status { get; set; }
}
