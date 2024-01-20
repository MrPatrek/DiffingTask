namespace Entities.Exceptions
{
    public sealed class CollectionByKeyIdsBadRequestException : BadRequestException
    {
        public CollectionByKeyIdsBadRequestException()
            : base("Collection count mismatch comparing to ids.")
        {
        }
    }
}
