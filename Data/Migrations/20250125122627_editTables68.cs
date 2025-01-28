using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class editTables68 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGlobal",
                table: "Spaces",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ModelAis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dialect",
                table: "ModelAis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsStandard",
                table: "ModelAis",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "ModelAis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ModelAis",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGlobal",
                table: "Spaces");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "ModelAis");

            migrationBuilder.DropColumn(
                name: "Dialect",
                table: "ModelAis");

            migrationBuilder.DropColumn(
                name: "IsStandard",
                table: "ModelAis");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "ModelAis");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ModelAis");
        }
    }
}
