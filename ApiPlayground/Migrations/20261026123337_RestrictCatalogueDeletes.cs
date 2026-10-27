using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPlayground.Migrations
{
    /// <inheritdoc />
    public partial class RestrictCatalogueDeletes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectionGames_Games_GameId",
                table: "CollectionGames");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryEntries_Games_GameId",
                table: "LibraryEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryEntries_Platforms_PlatformID",
                table: "LibraryEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionGames_Games_GameId",
                table: "CollectionGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryEntries_Games_GameId",
                table: "LibraryEntries",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryEntries_Platforms_PlatformID",
                table: "LibraryEntries",
                column: "PlatformID",
                principalTable: "Platforms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectionGames_Games_GameId",
                table: "CollectionGames");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryEntries_Games_GameId",
                table: "LibraryEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryEntries_Platforms_PlatformID",
                table: "LibraryEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionGames_Games_GameId",
                table: "CollectionGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryEntries_Games_GameId",
                table: "LibraryEntries",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryEntries_Platforms_PlatformID",
                table: "LibraryEntries",
                column: "PlatformID",
                principalTable: "Platforms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
