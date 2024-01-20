using Entities.Extensions;

namespace Entities.Exceptions
{
    public class EncodedBinaryNotFoundException : NotFoundException
    {
        public EncodedBinaryNotFoundException(uint keyId, bool side)
            : base($"EncodedBinary with keyId: {keyId} and side: {side.GetSideString()} - doesn't exist in the database.")
        {
        }
    }
}
