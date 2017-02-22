using System;

namespace Core.Common.Contracts
{
    public sealed class RepositoryActionResult<T> where T: class
    {
        public T Entity { get; }
        public RepositoryActionState State { get; }
        public Exception Exception { get; }

        public RepositoryActionResult(T entity, RepositoryActionState state)
        {
            Entity = entity;
            State = state;
        }

        public RepositoryActionResult(T entity, RepositoryActionState state, Exception exception)
            : this(entity, state)
        {
            Exception = exception;
        }
    }
}
