using AutoMapper;
using Entities.Extensions;
using Entities.Models;
using Shared.DataTransferObjects;

namespace DiffingTask
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Model to Dto:
            CreateMap<EncodedBinary, EncodedBinaryDto>()
                .ForMember(ebDto => ebDto.Side,
                    opt => opt.MapFrom(eb => eb.Side.GetSideString()));

            // Dto to Model:
            CreateMap<EncodedBinaryForCreationDto, EncodedBinary>();

            CreateMap<EncodedBinaryForCreationCollectionDto, EncodedBinary>()
                .ForMember(eb => eb.Side,
                    opt => opt.MapFrom(ebCreatCollecDto => ebCreatCollecDto.Side.GetSideBool()));

            CreateMap<EncodedBinaryForUpdateDto, EncodedBinary>()
                .ReverseMap();

            // Dto to Model: mapping inheritance
            CreateMap<EncodedBinaryForManipulationDto, EncodedBinary>()
                .IncludeAllDerived()
                .ForMember(eb => eb.Data,
                    opt => opt.MapFrom(ebManipDto => Convert.FromBase64String(ebManipDto.Data)));
        }
    }
}
