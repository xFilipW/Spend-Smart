using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsersApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAttachmentToExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AttachmentPath",
                table: "Expenses",
                newName: "AttachmentFileName");

            migrationBuilder.AddColumn<byte[]>(
                name: "AttachmentData",
                table: "Expenses",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentData",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "AttachmentFileName",
                table: "Expenses",
                newName: "AttachmentPath");
        }
    }
}
