using System;

namespace OneComic.Common
{
    public sealed class AuthorizationValidationException : ApplicationException
    {
        public AuthorizationValidationException(string message)
            : base(message)
        {
        }

        public AuthorizationValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
