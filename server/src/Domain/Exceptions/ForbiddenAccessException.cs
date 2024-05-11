using System.Net;

namespace Domain.Exceptions;

public class ForbiddenAccessException : ApplicationException
{
    public ForbiddenAccessException(string message)
        : base(HttpStatusCode.Forbidden, message)
    {
    }
}
