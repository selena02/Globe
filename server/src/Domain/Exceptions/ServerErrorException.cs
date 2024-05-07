using System.Net;

namespace Domain.Exceptions;

public class ServerErrorException: ApplicationException
{
    public ServerErrorException(string message)
        : base(HttpStatusCode.InternalServerError, message)
    {
    }
}