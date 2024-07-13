using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceNet8.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editNAme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainCategoryId",
                table: "BaseProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MainCategoryId",
                table: "BaseProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
