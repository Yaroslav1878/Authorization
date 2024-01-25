using Authorization.Domain.Contexts;
using Authorization.Domain.Models.Entities;
using Authorization.Domain.Repositories.Abstractions;

namespace Authorization.Domain.Repositories;

public class UserAuthRepository : GenericRepository<UserAuth>, IUserAuthRepository
{
    public UserAuthRepository(AuthContext context)
        : base(context.UserAuths)
    {
    }
}
