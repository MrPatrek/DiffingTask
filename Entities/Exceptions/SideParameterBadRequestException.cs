namespace Entities.Exceptions
{
    public sealed class SideParameterBadRequestException : BadRequestException
    {
        public SideParameterBadRequestException()
            : base("Parameter side must be either left or right (or diff for difference), and nothing else.")
        {
        }
    }
}
