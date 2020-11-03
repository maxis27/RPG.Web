using Microsoft.EntityFrameworkCore.Migrations;

namespace RPG.Web.Migrations
{
    public partial class AddUserStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Hours",
                table: "Tavern",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<float>(
                name: "Hours",
                table: "Tavern",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldNullable: true);
        }
    }
}
