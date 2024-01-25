using Authorization.Domain.Contexts;
using Authorization.Domain.Models.Entities;
using Authorization.Domain.Repositories.Abstractions;

namespace Authorization.Domain.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AuthContext context)
        : base(context.Users)
    {
    }
}
