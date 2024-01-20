using Entities.Extensions;

namespace Entities.Exceptions
{
    public class EncodedBinaryFoundBadRequestException : BadRequestException
    {
        public EncodedBinaryFoundBadRequestException(uint keyId, bool side)
            : base($"EncodedBinary with keyId: {keyId}, and side: {side.GetSideString()} - already exists in the database.")
        {
        }
    }
}
