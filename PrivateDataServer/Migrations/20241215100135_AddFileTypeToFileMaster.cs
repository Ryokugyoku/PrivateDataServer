using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrivateDataServer.Migrations
{
    /// <inheritdoc />
    public partial class AddFileTypeToFileMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "file_type",
                table: "file_master",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_type",
                table: "file_master");
        }
    }
}
