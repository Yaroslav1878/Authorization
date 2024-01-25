using System;
using System.Threading.Tasks;

namespace Authorization.Domain.Services.Abstraction
{
    public interface IFileService
    {
        Task DeleteFile(Guid fileKey);
    }
}