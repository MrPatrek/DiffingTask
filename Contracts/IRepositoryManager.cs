namespace Contracts
{
    public interface IRepositoryManager
    {
        IEncodedBinaryRepository EncodedBinary { get; }
        Task SaveAsync();
    }
}
