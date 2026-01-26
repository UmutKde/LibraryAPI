using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixBookCopyTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isAvailable",
                table: "BookCopies",
                newName: "IsAvailable");

            migrationBuilder.RenameColumn(
                name: "ReplacamentCost",
                table: "BookCopies",
                newName: "ReplacementCost");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "BookCopies",
                newName: "isAvailable");

            migrationBuilder.RenameColumn(
                name: "ReplacementCost",
                table: "BookCopies",
                newName: "ReplacamentCost");
        }
    }
}
