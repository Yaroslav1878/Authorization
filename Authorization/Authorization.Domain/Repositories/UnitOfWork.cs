using System.Threading.Tasks;
using Authorization.Domain.Contexts;
using Authorization.Domain.Repositories.Abstractions;

namespace Authorization.Domain.Repositories;

public class UnitOfWork(AuthContext context) : IUnitOfWork
{
    public Task Commit()
    {
        return context.SaveChangesAsync();
    }
}
