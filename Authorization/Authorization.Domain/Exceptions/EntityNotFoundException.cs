using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions;

public class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException(string tableName)
        : base(ErrorCode.EntityNotFoundException, HttpStatusCode.NotFound, $"{tableName} entity with specified identifier wasn't not found.")
    {
    }
}
