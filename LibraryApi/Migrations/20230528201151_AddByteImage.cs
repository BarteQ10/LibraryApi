using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApi.Migrations
{
    /// <inheritdoc />
    public partial class AddByteImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Books");

            migrationBuilder.AddColumn<byte[]>(
                name: "CoverImageData",
                table: "Books",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImageData",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Books",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
