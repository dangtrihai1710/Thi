using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thi.Migrations
{
    /// <inheritdoc />
    public partial class AddSoLuongToHocPhanTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                keyValue: "QTDK02",
                column: "SoLuong",
                value: 99);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "HocPhans",
                keyColumn: "MaHP",
                keyValue: "CNTT01",
                column: "SoLuong",
                value: 100);

            migrationBuilder.UpdateData(
                table: "HocPhans",
                keyColumn: "MaHP",
                keyValue: "CNTT02",
                column: "SoLuong",
                value: 100);

            migrationBuilder.UpdateData(
                table: "HocPhans",
                keyColumn: "MaHP",
                keyValue: "QTDK02",
                column: "SoLuong",
                value: 100);
        }
    }
}
