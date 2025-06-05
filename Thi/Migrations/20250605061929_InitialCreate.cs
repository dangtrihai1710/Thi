using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HocPhans",
                columns: table => new
                {
                    MaHP = table.Column<string>(type: "char(6)", nullable: false),
                    TenHP = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SoTinChi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocPhans", x => x.MaHP);
                });

            migrationBuilder.CreateTable(
                name: "NganhHocs",
                columns: table => new
                {
                    MaNganh = table.Column<string>(type: "char(4)", nullable: false),
                    TenNganh = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NganhHocs", x => x.MaNganh);
                });

            migrationBuilder.CreateTable(
                name: "SinhViens",
                columns: table => new
                {
                    MaSV = table.Column<string>(type: "char(10)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Hinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaNganh = table.Column<string>(type: "char(4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhViens", x => x.MaSV);
                    table.ForeignKey(
                        name: "FK_SinhViens_NganhHocs_MaNganh",
                        column: x => x.MaNganh,
                        principalTable: "NganhHocs",
                        principalColumn: "MaNganh");
                });

            migrationBuilder.CreateTable(
                name: "DangKys",
                columns: table => new
                {
                    MaDK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayDK = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaSV = table.Column<string>(type: "char(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKys", x => x.MaDK);
                    table.ForeignKey(
                        name: "FK_DangKys_SinhViens_MaSV",
                        column: x => x.MaSV,
                        principalTable: "SinhViens",
                        principalColumn: "MaSV");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDangKys",
                columns: table => new
                {
                    MaDK = table.Column<int>(type: "int", nullable: false),
                    MaHP = table.Column<string>(type: "char(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDangKys", x => new { x.MaDK, x.MaHP });
                    table.ForeignKey(
                        name: "FK_ChiTietDangKys_DangKys_MaDK",
                        column: x => x.MaDK,
                        principalTable: "DangKys",
                        principalColumn: "MaDK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDangKys_HocPhans_MaHP",
                        column: x => x.MaHP,
                        principalTable: "HocPhans",
                        principalColumn: "MaHP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "HocPhans",
                columns: new[] { "MaHP", "SoTinChi", "TenHP" },
                values: new object[,]
                {
                    { "CNTT01", 3, "Lập trình C" },
                    { "CNTT02", 2, "Cơ sở dữ liệu" },
                    { "QTDK02", 3, "Xác suất thống kê 1" },
                    { "QTKD01", 2, "Kinh tế vi mô" }
                });

            migrationBuilder.InsertData(
                table: "NganhHocs",
                columns: new[] { "MaNganh", "TenNganh" },
                values: new object[,]
                {
                    { "CNTT", "Công nghệ thông tin" },
                    { "QTKD", "Quản trị kinh doanh" }
                });

            migrationBuilder.InsertData(
                table: "SinhViens",
                columns: new[] { "MaSV", "GioiTinh", "Hinh", "HoTen", "MaNganh", "NgaySinh" },
                values: new object[,]
                {
                    { "2280600791", "Nam", "/Content/images/sv1.jpg", "Đặng Trí Hải", "CNTT", new DateTime(2004, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "9876543210", "Nữ", "/Content/images/sv2.jpg", "Nguyễn Thị B", "QTKD", new DateTime(2000, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDangKys_MaHP",
                table: "ChiTietDangKys",
                column: "MaHP");

            migrationBuilder.CreateIndex(
                name: "IX_DangKys_MaSV",
                table: "DangKys",
                column: "MaSV");

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_MaNganh",
                table: "SinhViens",
                column: "MaNganh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDangKys");

            migrationBuilder.DropTable(
                name: "DangKys");

            migrationBuilder.DropTable(
                name: "HocPhans");

            migrationBuilder.DropTable(
                name: "SinhViens");

            migrationBuilder.DropTable(
                name: "NganhHocs");
        }
    }
}
