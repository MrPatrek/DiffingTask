namespace Shared.DataTransferObjects
{
    public record EncodedBinaryDto
    {
        public uint KeyId { get; init; }
        public string? Side { get; init; }
        public byte[]? Data { get; init; }
    }
}
