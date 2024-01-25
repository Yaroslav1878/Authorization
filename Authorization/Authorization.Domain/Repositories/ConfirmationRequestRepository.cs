using Authorization.Domain.Contexts;
using Authorization.Domain.Models.Entities;
using Authorization.Domain.Repositories.Abstractions;

namespace Authorization.Domain.Repositories;

public class ConfirmationRequestRepository : GenericRepository<ConfirmationRequest>, IConfirmationRequestRepository
{
    public ConfirmationRequestRepository(AuthContext context)
        : base(context.ConfirmationRequests)
    {
    }
}
