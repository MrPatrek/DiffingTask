namespace Entities.Exceptions
{
    public sealed class EncodedBinaryCollectionBadRequest : BadRequestException
    {
        public EncodedBinaryCollectionBadRequest()
            : base("EncodedBinary collection sent from a client is null or empty.")
        {
        }
    }
}
