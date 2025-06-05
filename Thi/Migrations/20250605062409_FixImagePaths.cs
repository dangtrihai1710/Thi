using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thi.Migrations
{
    /// <inheritdoc />
    public partial class FixImagePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SinhViens",
                keyColumn: "MaSV",
                keyValue: "2280600791",
                column: "Hinh",
                value: "/images/sv1.jpg");

            migrationBuilder.UpdateData(
                table: "SinhViens",
                keyColumn: "MaSV",
                keyValue: "9876543210",
                column: "Hinh",
                value: "/images/sv2.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SinhViens",
                keyColumn: "MaSV",
                keyValue: "2280600791",
                column: "Hinh",
                value: "/Content/images/sv1.jpg");

            migrationBuilder.UpdateData(
                table: "SinhViens",
                keyColumn: "MaSV",
                keyValue: "9876543210",
                column: "Hinh",
                value: "/Content/images/sv2.jpg");
        }
    }
}
