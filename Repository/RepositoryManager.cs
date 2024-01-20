using Contracts;

namespace Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IEncodedBinaryRepository> _encodedBinaryRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _encodedBinaryRepository = new Lazy<IEncodedBinaryRepository>(() => new EncodedBinaryRepository(repositoryContext));
        }

        public IEncodedBinaryRepository EncodedBinary => _encodedBinaryRepository.Value;

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
