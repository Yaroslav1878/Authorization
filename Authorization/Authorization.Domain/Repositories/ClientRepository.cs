using Authorization.Domain.Contexts;
using Authorization.Domain.Models.Entities;
using Authorization.Domain.Repositories.Abstractions;

namespace Authorization.Domain.Repositories;

public class ClientRepository : GenericRepository<Client>, IClientRepository
{
    public ClientRepository(AuthContext context)
        : base(context.Clients)
    {
    }
}