using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicRecordsApp.Migrations
{
    public partial class UpdatedStudentsCoursesCompositeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentsCourses",
                table: "StudentsCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudentsCourses_StudentsId",
                table: "StudentsCourses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentsCourses",
                table: "StudentsCourses",
                columns: new[] { "StudentsId", "CoursesId" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentsCourses_CoursesId",
                table: "StudentsCourses",
                column: "CoursesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentsCourses",
                table: "StudentsCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudentsCourses_CoursesId",
                table: "StudentsCourses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentsCourses",
                table: "StudentsCourses",
                columns: new[] { "CoursesId", "StudentsId" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentsCourses_StudentsId",
                table: "StudentsCourses",
                column: "StudentsId");
        }
    }
}
