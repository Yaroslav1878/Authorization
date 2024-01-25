using System.Net;
using Authorization.Domain.Enums;

namespace Authorization.Domain.Exceptions;

public class DuplicateEmailException : ApplicationException
{
    private const string DuplicateEmail = "Duplicate email: ";

    public DuplicateEmailException(string email)
        : base(ErrorCode.DuplicatedEmailException, HttpStatusCode.Conflict, DuplicateEmail + email)
    {
    }
}
