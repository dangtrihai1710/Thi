using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSinhVienHinhLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SinhViens_NganhHocs_MaNganh",
                table: "SinhViens");

            migrationBuilder.AlterColumn<string>(
                name: "MaNganh",
                table: "SinhViens",
                type: "char(4)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "char(4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Hinh",
                table: "SinhViens",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SinhViens_NganhHocs_MaNganh",
                table: "SinhViens",
                column: "MaNganh",
                principalTable: "NganhHocs",
                principalColumn: "MaNganh",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SinhViens_NganhHocs_MaNganh",
                table: "SinhViens");

            migrationBuilder.AlterColumn<string>(
                name: "MaNganh",
                table: "SinhViens",
                type: "char(4)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(4)");

            migrationBuilder.AlterColumn<string>(
                name: "Hinh",
                table: "SinhViens",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SinhViens_NganhHocs_MaNganh",
                table: "SinhViens",
                column: "MaNganh",
                principalTable: "NganhHocs",
                principalColumn: "MaNganh");
        }
    }
}
