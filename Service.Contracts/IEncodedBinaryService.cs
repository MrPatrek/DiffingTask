using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts
{
    public interface IEncodedBinaryService
    {
        Task<(IEnumerable<EncodedBinaryDto> encodedBinaries, MetaData metaData)> GetEncodedBinariesAsync
            (EncodedBinaryParameters encodedBinaryParameters, bool trackChanges);
        Task<EncodedBinaryDto> GetEncodedBinaryAsync(uint keyId, string side, bool trackChanges);
        Task<EncodedBinaryDto> CreateEncodedBinaryAsync(uint keyId, string side, EncodedBinaryForCreationDto encodedBinaryForCreation, bool trackChanges);
        Task DeleteEncodedBinaryAsync(uint keyId, string side, bool trackChanges);
        Task UpdateEncodedBinaryAsync(uint keyId, string side,
            EncodedBinaryForUpdateDto encodedBinaryForUpdate, bool trackChanges);

        // Get diff
        Task<DiffResultDto> GetDiffAsync(uint keyId, bool trackChanges);

        // Collection
        Task<IEnumerable<EncodedBinaryDto>> GetByKeyIdsAsync(IEnumerable<uint> keyIds, bool trackChanges);
        Task<(IEnumerable<EncodedBinaryDto> encodedBinaries, string keyIds)> CreateEncodedBinaryCollectionAsync
            (IEnumerable<EncodedBinaryForCreationCollectionDto> encodedBinaryCollection, bool trackChanges);

        // PATCH request
        Task<(EncodedBinaryForUpdateDto encodedBinaryToPatch, EncodedBinary encodedBinaryEntity)> GetEncodedBinaryForPatchAsync
            (uint keyId, string side, bool trackChanges);
        Task SaveChangesForPatchAsync(EncodedBinaryForUpdateDto encodedBinaryToPatch, EncodedBinary encodedBinaryEntity);
    }
}
