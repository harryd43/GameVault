using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPlayground.Migrations
{
    /// <inheritdoc />
    public partial class UniqueLibraryEntryPerGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LibraryEntries_UserId",
                table: "LibraryEntries");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryEntries_UserId_GameId",
                table: "LibraryEntries",
                columns: new[] { "UserId", "GameId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LibraryEntries_UserId_GameId",
                table: "LibraryEntries");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryEntries_UserId",
                table: "LibraryEntries",
                column: "UserId");
        }
    }
}
