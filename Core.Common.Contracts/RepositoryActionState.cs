namespace Core.Common.Contracts
{
    public enum RepositoryActionState
    {
        Ok,
        Created,
        Updated,
        NotFound,
        Deleted,
        NothingModified,
        Error
    }
}