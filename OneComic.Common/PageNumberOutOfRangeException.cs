using System;

namespace OneComic.Common
{
    public sealed class PageNumberOutOfRangeException : ApplicationException
    {
        public PageNumberOutOfRangeException(string message)
            : base(message)
        {
        }

        public PageNumberOutOfRangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
