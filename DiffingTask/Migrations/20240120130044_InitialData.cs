using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DiffingTask.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EncodedBinaries",
                columns: table => new
                {
                    EncodedBinaryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KeyId = table.Column<long>(type: "bigint", nullable: false),
                    Side = table.Column<bool>(type: "bit", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncodedBinaries", x => x.EncodedBinaryId);
                });

            migrationBuilder.InsertData(
                table: "EncodedBinaries",
                columns: new[] { "EncodedBinaryId", "Data", "KeyId", "Side" },
                values: new object[,]
                {
                    { new Guid("403c0f94-5614-4161-bf38-654ca4892321"), new byte[] { 49, 48, 49, 32, 108, 101, 102, 116, 32, 105, 115, 32, 101, 113, 117, 97, 108, 32, 116, 111, 32, 49, 48, 49, 32, 114, 105, 103, 104, 116 }, 101L, true },
                    { new Guid("6bbca686-3ff6-4bdc-8598-d4311c5783e7"), new byte[] { 97, 98, 99, 68, 69, 70, 103, 104, 73, 106, 107, 108, 77, 78, 79, 80, 81, 114, 115, 116, 85, 86, 119, 120, 89, 122, 32, 45, 32, 115, 97, 109, 101, 32, 108, 101, 110, 44, 32, 100, 105, 102, 102, 32, 115, 116, 114 }, 103L, true },
                    { new Guid("a37a2ae0-20e6-4d7c-83c5-f9a2d41dd534"), new byte[] { 49, 48, 49, 32, 108, 101, 102, 116, 32, 105, 115, 32, 101, 113, 117, 97, 108, 32, 116, 111, 32, 49, 48, 49, 32, 114, 105, 103, 104, 116 }, 101L, false },
                    { new Guid("d10ce1c7-bdac-4e86-91e0-230b9f003e12"), new byte[] { 49, 48, 50, 32, 114, 105, 103, 104, 116, 32, 100, 105, 102, 102, 101, 114, 101, 110, 116, 32, 115, 116, 114, 105, 110, 103, 32, 115, 105, 122, 101, 46, 46, 46 }, 102L, true },
                    { new Guid("ee3de60c-2dfa-4ee5-be35-18564439f452"), new byte[] { 49, 48, 50, 32, 108, 101, 102, 116, 32, 100, 105, 102, 102, 101, 114, 101, 110, 116, 32, 115, 116, 114, 105, 110, 103, 32, 115, 105, 122, 101 }, 102L, false },
                    { new Guid("f2e82c71-4008-42e0-b91e-3b88b3a6c7bb"), new byte[] { 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 32, 45, 32, 115, 97, 109, 101, 32, 108, 101, 110, 44, 32, 100, 105, 102, 102, 32, 115, 116, 114 }, 103L, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EncodedBinaries");
        }
    }
}
