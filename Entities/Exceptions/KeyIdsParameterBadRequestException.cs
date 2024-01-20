namespace Entities.Exceptions
{
    public sealed class KeyIdsParameterBadRequestException : BadRequestException
    {
        public KeyIdsParameterBadRequestException()
            : base("Parameter keyIds is null.")
        {
        }
    }
}
