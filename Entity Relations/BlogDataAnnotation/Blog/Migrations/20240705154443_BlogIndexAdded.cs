using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogDataAnnotation.Migrations
{
    public partial class BlogIndexAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Name_Unique",
                schema: "blg",
                table: "Blogs",
                column: "BlogName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Blogs_Name_Unique",
                schema: "blg",
                table: "Blogs");
        }
    }
}
