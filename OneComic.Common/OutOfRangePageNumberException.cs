using System;

namespace OneComic.Common
{
    public sealed class OutOfRangePageNumberException : ApplicationException
    {
        public OutOfRangePageNumberException(string message)
            : base(message)
        {
        }

        public OutOfRangePageNumberException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
