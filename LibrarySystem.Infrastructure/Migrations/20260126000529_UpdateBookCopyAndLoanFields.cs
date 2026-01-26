using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookCopyAndLoanFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "BorrowedAt",
                table: "Loans",
                newName: "LoanDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoanDate",
                table: "Loans",
                newName: "BorrowedAt");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Loans",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
