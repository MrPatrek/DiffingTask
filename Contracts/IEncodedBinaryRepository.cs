using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts
{
    public interface IEncodedBinaryRepository
    {
        Task<PagedList<EncodedBinary>> GetEncodedBinariesAsync(EncodedBinaryParameters encodedBinaryParameters, bool trackChanges);
        Task<IEnumerable<EncodedBinary>> GetByKeyIdsAsync(IEnumerable<uint> keyIds, bool trackChanges);
        Task<EncodedBinary> GetEncodedBinaryAsync(uint encodedBinaryKeyId, bool side, bool trackChanges);
        void CreateEncodedBinary(EncodedBinary encodedBinary);
        void DeleteEncodedBinary(EncodedBinary encodedBinary);
    }
}
