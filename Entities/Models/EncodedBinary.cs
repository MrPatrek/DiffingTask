using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class EncodedBinary
    {
        [Column("EncodedBinaryId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "KeyId is a required field.")]
        [Range(1, uint.MaxValue, ErrorMessage = "KeyId should be a positive integer.")]
        public uint KeyId { get; set; }

        [Required(ErrorMessage = "Side is a required field.")]
        public bool Side { get; set; }      // left side is 0, right side is 1

        [Required(ErrorMessage = "Data is a required field.")]
        public byte[]? Data { get; set; }
    }
}
