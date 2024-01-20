using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public abstract record EncodedBinaryForManipulationDto
    {
        [Required(ErrorMessage = "Data is a required field.")]
        [Base64String]
        public string? Data { get; init; }
    }
}
