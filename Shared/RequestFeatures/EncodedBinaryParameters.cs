namespace Shared.RequestFeatures
{
    public class EncodedBinaryParameters : RequestParameters
    {
        public EncodedBinaryParameters() => OrderBy = "keyId";        // default value if no other value is set

        public uint MinKeyId { get; set; } = 1;
        public uint MaxKeyId { get; set; } = uint.MaxValue;

        public bool ValidKeyIdRange => MaxKeyId > MinKeyId;

        public string? SearchTerm { get; set; }
    }
}
