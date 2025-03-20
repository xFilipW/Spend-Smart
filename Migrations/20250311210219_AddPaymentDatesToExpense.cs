using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsersApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentDatesToExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Expenses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDueDate",
                table: "Expenses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "PaymentDueDate",
                table: "Expenses");
        }
    }
}
