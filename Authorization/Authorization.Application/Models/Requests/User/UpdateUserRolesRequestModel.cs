using System.Collections.Generic;

namespace Authorization.Application.Models.Requests.User;

public class UpdateUserRolesRequestModel
{
    public IReadOnlyCollection<string> RoleIds { get; set; }
}
