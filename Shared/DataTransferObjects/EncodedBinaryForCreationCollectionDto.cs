using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record EncodedBinaryForCreationCollectionDto : EncodedBinaryForCreationDto
    {
        //[Required(ErrorMessage = "KeyId is a required field.")]
        // Required field above anyway will not help since even if we do not pass value to it,
        // value will be automatically set to 0 (since it's int (uint)), so for this use the
        // [Range(0, ......)] attribute (which is already present below):
        [Range(1, uint.MaxValue, ErrorMessage = "KeyId should be a positive integer.")]
        public uint KeyId { get; init; }

        [Required(ErrorMessage = "Side is a required field.")]
        public string? Side { get; init; }      // left side is 0, right side is 1
    }
}
