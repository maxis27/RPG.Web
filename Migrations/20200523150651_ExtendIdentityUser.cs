using Microsoft.EntityFrameworkCore.Migrations;

namespace RPG.Web.Migrations
{
    public partial class ExtendIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActionPoints",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AllExperience",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dexterity",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExperiencePoints",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Gold",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HealthPoints",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Intelligence",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxActionPoints",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxExperiencePoints",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxHealthPoints",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SkillPoints",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stamina",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Strength",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionPoints",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AllExperience",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Dexterity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ExperiencePoints",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HealthPoints",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Intelligence",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaxActionPoints",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaxExperiencePoints",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaxHealthPoints",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SkillPoints",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Stamina",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Strength",
                table: "AspNetUsers");
        }
    }
}
