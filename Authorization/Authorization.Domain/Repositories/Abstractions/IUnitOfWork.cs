using System.Threading.Tasks;

namespace Authorization.Domain.Repositories.Abstractions;

public interface IUnitOfWork
{
    public Task Commit();
}
