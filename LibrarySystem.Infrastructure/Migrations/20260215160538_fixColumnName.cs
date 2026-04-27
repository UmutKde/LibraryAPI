using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefrestTokenExpiryTime",
                table: "AspNetUsers",
                newName: "RefreshTokenExpiryTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                newName: "RefrestTokenExpiryTime");
        }
    }
}
