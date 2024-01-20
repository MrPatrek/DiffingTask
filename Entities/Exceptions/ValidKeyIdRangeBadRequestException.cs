namespace Entities.Exceptions
{
    public sealed class ValidKeyIdRangeBadRequestException : BadRequestException
    {
        public ValidKeyIdRangeBadRequestException()
            : base("Max KeyId can't be less than min KeyId.")
        {
        }
    }
}
