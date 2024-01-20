using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Extensions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service
{
    internal sealed class EncodedBinaryService : IEncodedBinaryService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EncodedBinaryService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<EncodedBinaryDto> encodedBinaries, MetaData metaData)> GetEncodedBinariesAsync
            (EncodedBinaryParameters encodedBinaryParameters, bool trackChanges)
        {
            if (!encodedBinaryParameters.ValidKeyIdRange)
                throw new ValidKeyIdRangeBadRequestException();

            var encodedBinariesWithMetaData = await _repository.EncodedBinary.GetEncodedBinariesAsync(encodedBinaryParameters, trackChanges);
            var encodedBinariesDto = _mapper.Map<IEnumerable<EncodedBinaryDto>>(encodedBinariesWithMetaData);

            return (encodedBinaries: encodedBinariesDto, metaData: encodedBinariesWithMetaData.MetaData);
        }

        public async Task<EncodedBinaryDto> GetEncodedBinaryAsync(uint keyId, string side, bool trackChanges)
        {
            var encodedBinary = await GetEncodedBinaryAndExpectItExists(keyId, side.GetSideBool(), trackChanges);
            var encodedBinaryDto = _mapper.Map<EncodedBinaryDto>(encodedBinary);

            return encodedBinaryDto;
        }

        public async Task<EncodedBinaryDto> CreateEncodedBinaryAsync(uint keyId, string side, EncodedBinaryForCreationDto encodedBinaryForCreation, bool trackChanges)
        {
            bool sideBool = side.GetSideBool();

            await GetEncodedBinaryAndExpectItDoesNotExist(keyId, sideBool, trackChanges);

            var encodedBinaryEntity = _mapper.Map<EncodedBinary>(encodedBinaryForCreation);
            encodedBinaryEntity.KeyId = keyId;
            encodedBinaryEntity.Side = sideBool;

            _repository.EncodedBinary.CreateEncodedBinary(encodedBinaryEntity);
            await _repository.SaveAsync();

            var encodedBinaryToReturn = _mapper.Map<EncodedBinaryDto>(encodedBinaryEntity);

            return encodedBinaryToReturn;
        }

        public async Task DeleteEncodedBinaryAsync(uint keyId, string side, bool trackChanges)
        {
            var encodedBinary = await GetEncodedBinaryAndExpectItExists(keyId, side.GetSideBool(), trackChanges);

            _repository.EncodedBinary.DeleteEncodedBinary(encodedBinary);
            await _repository.SaveAsync();
        }

        public async Task UpdateEncodedBinaryAsync(uint keyId, string side, EncodedBinaryForUpdateDto encodedBinaryForUpdate, bool trackChanges)
        {
            var encodedBinary = await GetEncodedBinaryAndExpectItExists(keyId, side.GetSideBool(), trackChanges);

            _mapper.Map(encodedBinaryForUpdate, encodedBinary);
            await _repository.SaveAsync();
        }





        // Get diff

        public async Task<DiffResultDto> GetDiffAsync(uint keyId, bool trackChanges)
        {
            var encodedBinaryLeft = await GetEncodedBinaryAndExpectItExists(keyId, "left".GetSideBool(), trackChanges);
            var encodedBinaryRight = await GetEncodedBinaryAndExpectItExists(keyId, "right".GetSideBool(), trackChanges);

            string dataLeft = Convert.ToBase64String(encodedBinaryLeft.Data);
            string dataRight = Convert.ToBase64String(encodedBinaryRight.Data);

            string diffResultType;
            List<DiffDto>? diffs = null;



            if (dataLeft.Length != dataRight.Length)
                diffResultType = "SizeDoNotMatch";

            else
            {
                if (dataLeft.Equals(dataRight))
                    diffResultType = "Equals";

                else
                {
                    diffResultType = "ContentDoNotMatch";
                    diffs = [];

                    int notEqualCount = 0;
                    for (int i = 0; i < dataLeft.Length; i++)       // dataLeft len is the same as dataRight len, so doesn't matter which one to use
                    {
                        if (dataLeft[i] != dataRight[i])
                            notEqualCount++;

                        else
                            CheckEqualCount(ref notEqualCount, diffs, i);
                    }

                    CheckEqualCount(ref notEqualCount, diffs, dataLeft.Length);
                }
            }



            return new DiffResultDto
            {
                DiffResultType = diffResultType,
                Diffs = diffs
            };
        }





        // Collection

        public async Task<IEnumerable<EncodedBinaryDto>> GetByKeyIdsAsync(IEnumerable<uint> keyIds, bool trackChanges)
        {
            if (keyIds is null)
                throw new KeyIdsParameterBadRequestException();

            var encodedBinaryEntities = await _repository.EncodedBinary.GetByKeyIdsAsync(keyIds, trackChanges);
            // I don't think we need it here:
            //if (keyIds.Count() != encodedBinaryEntities.Count())
            //    throw new CollectionByKeyIdsBadRequestException();

            var encodedBinariesToReturn = _mapper.Map<IEnumerable<EncodedBinaryDto>>(encodedBinaryEntities);

            return encodedBinariesToReturn;
        }

        public async Task<(IEnumerable<EncodedBinaryDto> encodedBinaries, string keyIds)> CreateEncodedBinaryCollectionAsync
            (IEnumerable<EncodedBinaryForCreationCollectionDto> encodedBinaryCollection, bool trackChanges)
        {
            if (encodedBinaryCollection is null || !encodedBinaryCollection.Any())
                throw new EncodedBinaryCollectionBadRequest();

            var encodedBinaryEntities = _mapper.Map<IEnumerable<EncodedBinary>>(encodedBinaryCollection);
            foreach (var encodedBinary in encodedBinaryEntities)
            {
                await GetEncodedBinaryAndExpectItDoesNotExist(encodedBinary.KeyId, encodedBinary.Side, trackChanges);
                _repository.EncodedBinary.CreateEncodedBinary(encodedBinary);
            }
            await _repository.SaveAsync();

            var encodedBinaryCollectionToReturn = _mapper.Map<IEnumerable<EncodedBinaryDto>>(encodedBinaryEntities);
            var keyIds = string.Join(",", encodedBinaryCollectionToReturn.Select(c => c.KeyId).Distinct());
            // Discinct() here so that we get rid or keyId repetition (since we have two "sides" for each keyId)

            return (encodedBinaries: encodedBinaryCollectionToReturn, keyIds: keyIds);
        }





        // PATCH request

        public async Task<(EncodedBinaryForUpdateDto encodedBinaryToPatch, EncodedBinary encodedBinaryEntity)> GetEncodedBinaryForPatchAsync
            (uint keyId, string side, bool trackChanges)
        {
            var encodedBinaryDb = await GetEncodedBinaryAndExpectItExists(keyId, side.GetSideBool(), trackChanges);

            var encodedBinaryToPatch = _mapper.Map<EncodedBinaryForUpdateDto>(encodedBinaryDb);

            return (encodedBinaryToPatch: encodedBinaryToPatch, encodedBinaryEntity: encodedBinaryDb);
        }

        public async Task SaveChangesForPatchAsync(EncodedBinaryForUpdateDto encodedBinaryToPatch, EncodedBinary encodedBinaryEntity)
        {
            _mapper.Map(encodedBinaryToPatch, encodedBinaryEntity);
            await _repository.SaveAsync();
        }





        // Private stuff

        private async Task<EncodedBinary> GetEncodedBinaryAndExpectItExists(uint keyId, bool side, bool trackChanges)
        {
            var encodedBinaryDb = await _repository.EncodedBinary.GetEncodedBinaryAsync(keyId, side, trackChanges);
            if (encodedBinaryDb is not null)
                return encodedBinaryDb;             // desired outcome

            throw new EncodedBinaryNotFoundException(keyId, side);
        }

        private async Task GetEncodedBinaryAndExpectItDoesNotExist(uint keyId, bool side, bool trackChanges)
        {
            var encodedBinaryDb = await _repository.EncodedBinary.GetEncodedBinaryAsync(keyId, side, trackChanges);
            if (encodedBinaryDb is null)
                return;                             // desired outcome

            throw new EncodedBinaryFoundBadRequestException(keyId, side);
        }

        private static void CheckEqualCount(ref int notEqualCount, List<DiffDto> diffs, int i)
        {
            if (notEqualCount > 0)
            {
                diffs.Add(new DiffDto
                {
                    Offset = i - notEqualCount,
                    Length = notEqualCount
                });

                notEqualCount = 0;
            }
        }
    }
}
