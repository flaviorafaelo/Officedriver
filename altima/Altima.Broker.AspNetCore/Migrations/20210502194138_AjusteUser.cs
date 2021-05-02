using Microsoft.EntityFrameworkCore.Migrations;

namespace Altima.Broker.AspNet.Mvc.Migrations
{
    public partial class AjusteUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Owner",
                table: "User",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "User");
        }
    }
}
