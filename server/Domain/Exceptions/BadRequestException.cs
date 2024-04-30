using System.Net;

namespace Domain.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message)
            : base(HttpStatusCode.BadRequest, message)
        {
        }
    }
}
