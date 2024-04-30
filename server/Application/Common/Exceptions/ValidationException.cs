using System.Net;
using ApplicationException = Domain.Exceptions.ApplicationException;

namespace Application.Common.Exceptions
{
    public sealed class ValidationException : ApplicationException
    {
        public IReadOnlyDictionary<string, string[]> ValidationErrors { get; }

        public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary)
            : base(HttpStatusCode.BadRequest, "Validation Errors Occurred")
        {
            ValidationErrors = errorsDictionary;
        }
    }
}
