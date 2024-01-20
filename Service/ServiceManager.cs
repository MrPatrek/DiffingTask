using AutoMapper;
using Contracts;
using Service.Contracts;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IEncodedBinaryService> _encodedBinaryService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
        {
            _encodedBinaryService = new Lazy<IEncodedBinaryService>(() =>
                new EncodedBinaryService(repositoryManager, logger, mapper));
        }

        public IEncodedBinaryService EncodedBinaryService => _encodedBinaryService.Value;
    }
}
