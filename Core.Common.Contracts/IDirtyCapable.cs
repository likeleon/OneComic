namespace Core.Common.Contracts
{
    public interface IDirtyCapable
    {
        bool IsDirty { get; }
    }
}
