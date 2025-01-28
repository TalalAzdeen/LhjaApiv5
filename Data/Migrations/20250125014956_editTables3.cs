using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class editTables3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubscriptionId",
                table: "Spaces",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Spaces_SubscriptionId",
                table: "Spaces",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Spaces_Subscriptions_SubscriptionId",
                table: "Spaces",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spaces_Subscriptions_SubscriptionId",
                table: "Spaces");

            migrationBuilder.DropIndex(
                name: "IX_Spaces_SubscriptionId",
                table: "Spaces");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Spaces");
        }
    }
}
