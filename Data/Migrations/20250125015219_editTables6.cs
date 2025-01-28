using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class editTables6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Spaces");

            migrationBuilder.AddColumn<string>(
                name: "SpaceId",
                table: "Requests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_SpaceId",
                table: "Requests",
                column: "SpaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Spaces_SpaceId",
                table: "Requests",
                column: "SpaceId",
                principalTable: "Spaces",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Spaces_SpaceId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_SpaceId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SpaceId",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Spaces",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
