using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thi.Migrations
{
    /// <inheritdoc />
    public partial class AddSoLuongToHocPhan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoLuong",
                table: "HocPhans",
                type: "int",
                nullable: true);

            // Cập nhật dữ liệu mẫu với số lượng
            migrationBuilder.UpdateData(
                table: "HocPhans",
                keyColumn: "MaHP",
                keyValue: "CNTT01",
                column: "SoLuong",
                value: 99);

            migrationBuilder.UpdateData(
                table: "HocPhans",
                keyColumn: "MaHP",
                keyValue: "CNTT02",
                column: "SoLuong",
                value: 99);

            migrationBuilder.UpdateData(
                table: "HocPhans",
                keyColumn: "MaHP",
                keyValue: "QTKD01",
                column: "SoLuong",
                value: 100);

            migrationBuilder.UpdateData(
                table: "HocPhans",
                keyColumn: "MaHP",
                keyValue: "QTDK02",
                column: "SoLuong",
                value: 99);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoLuong",
                table: "HocPhans");
        }
    }
}