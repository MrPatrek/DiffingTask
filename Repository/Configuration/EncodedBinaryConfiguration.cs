using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class EncodedBinaryConfiguration : IEntityTypeConfiguration<EncodedBinary>
    {
        public void Configure(EntityTypeBuilder<EncodedBinary> builder)
        {
            builder.HasData
            (
                new EncodedBinary
                {
                    Id = new Guid("a37a2ae0-20e6-4d7c-83c5-f9a2d41dd534"),
                    KeyId = 101,
                    Side = false,
                    Data = Convert.FromBase64String("MTAxIGxlZnQgaXMgZXF1YWwgdG8gMTAxIHJpZ2h0")         // 101 left is equal to 101 right
                },
                new EncodedBinary
                {
                    Id = new Guid("403c0f94-5614-4161-bf38-654ca4892321"),
                    KeyId = 101,
                    Side = true,
                    Data = Convert.FromBase64String("MTAxIGxlZnQgaXMgZXF1YWwgdG8gMTAxIHJpZ2h0")         // 101 left is equal to 101 right
                },
                new EncodedBinary
                {
                    Id = new Guid("ee3de60c-2dfa-4ee5-be35-18564439f452"),
                    KeyId = 102,
                    Side = false,
                    Data = Convert.FromBase64String("MTAyIGxlZnQgZGlmZmVyZW50IHN0cmluZyBzaXpl")         // 102 left different string size
                },
                new EncodedBinary
                {
                    Id = new Guid("d10ce1c7-bdac-4e86-91e0-230b9f003e12"),
                    KeyId = 102,
                    Side = true,
                    Data = Convert.FromBase64String("MTAyIHJpZ2h0IGRpZmZlcmVudCBzdHJpbmcgc2l6ZS4uLg==")         // 102 right different string size...
                },
                new EncodedBinary
                {
                    Id = new Guid("f2e82c71-4008-42e0-b91e-3b88b3a6c7bb"),
                    KeyId = 103,
                    Side = false,
                    Data = Convert.FromBase64String("YWJjZGVmZ2hpamtsbW5vcHFyc3R1dnd4eXogLSBzYW1lIGxlbiwgZGlmZiBzdHI=")         // abcdefghijklmnopqrstuvwxyz - same len, diff str
                },
                new EncodedBinary
                {
                    Id = new Guid("6bbca686-3ff6-4bdc-8598-d4311c5783e7"),
                    KeyId = 103,
                    Side = true,
                    Data = Convert.FromBase64String("YWJjREVGZ2hJamtsTU5PUFFyc3RVVnd4WXogLSBzYW1lIGxlbiwgZGlmZiBzdHI=")         // abcDEFghIjklMNOPQrstUVwxYz - same len, diff str
                }
            );
        }
    }
}
