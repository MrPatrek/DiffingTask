using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;

namespace Repository
{
    public class EncodedBinaryRepository : RepositoryBase<EncodedBinary>, IEncodedBinaryRepository
    {
        public EncodedBinaryRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<PagedList<EncodedBinary>> GetEncodedBinariesAsync(EncodedBinaryParameters encodedBinaryParameters, bool trackChanges)
        {
            var binaries = await FindAll(trackChanges)
                .FilterEncodedBinaries(encodedBinaryParameters.MinKeyId, encodedBinaryParameters.MaxKeyId)
                //.Search(encodedBinaryParameters.SearchTerm)
                .Sort(encodedBinaryParameters.OrderBy)
                .Skip((encodedBinaryParameters.PageNumber - 1) * encodedBinaryParameters.PageSize)
                .Take(encodedBinaryParameters.PageSize)
                .ToListAsync();

            var count = await FindAll(trackChanges)
                .FilterEncodedBinaries(encodedBinaryParameters.MinKeyId, encodedBinaryParameters.MaxKeyId)
                //.Search(encodedBinaryParameters.SearchTerm)
                // do note that here I do not include .Sort() since it is not relevant for count ...
                .CountAsync();

            return new PagedList<EncodedBinary>(binaries, count, encodedBinaryParameters.PageNumber, encodedBinaryParameters.PageSize);
        }

        public async Task<IEnumerable<EncodedBinary>> GetByKeyIdsAsync(IEnumerable<uint> keyIds, bool trackChanges) =>
            await FindByCondition(eb => keyIds.Contains(eb.KeyId), trackChanges)
                .ToListAsync();

        public async Task<EncodedBinary> GetEncodedBinaryAsync(uint encodedBinaryKeyId, bool side, bool trackChanges) =>
            await FindByCondition(eb => eb.KeyId.Equals(encodedBinaryKeyId) && eb.Side.Equals(side), trackChanges)
                .SingleOrDefaultAsync();

        public void CreateEncodedBinary(EncodedBinary encodedBinary) =>
            Create(encodedBinary);

        public void DeleteEncodedBinary(EncodedBinary encodedBinary) =>
            Delete(encodedBinary);
    }
}
