using System.Net;

namespace Domain.Exceptions
{
    public class ApplicationException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApplicationException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
