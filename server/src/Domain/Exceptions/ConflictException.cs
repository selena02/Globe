using System.Net;

namespace Domain.Exceptions;

public class ConflictException : ApplicationException
{
    public ConflictException(string message)
        : base(HttpStatusCode.Conflict, message)
    {
    }
}