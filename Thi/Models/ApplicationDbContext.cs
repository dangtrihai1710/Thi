using Microsoft.EntityFrameworkCore;
using Thi.Models;

namespace Thi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<NganhHoc> NganhHocs { get; set; }
        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<HocPhan> HocPhans { get; set; }
        public DbSet<DangKy> DangKys { get; set; }
        public DbSet<ChiTietDangKy> ChiTietDangKys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite key for ChiTietDangKy
            modelBuilder.Entity<ChiTietDangKy>()
                .HasKey(ct => new { ct.MaDK, ct.MaHP });

            // Configure column types and lengths
            modelBuilder.Entity<SinhVien>()
                .Property(s => s.Hinh)
                .HasMaxLength(200); // Tăng độ dài cột Hinh

            // Configure relationships
            modelBuilder.Entity<SinhVien>()
                .HasOne(s => s.NganhHoc)
                .WithMany(n => n.SinhViens)
                .HasForeignKey(s => s.MaNganh);

            modelBuilder.Entity<DangKy>()
                .HasOne(d => d.SinhVien)
                .WithMany(s => s.DangKys)
                .HasForeignKey(d => d.MaSV);

            modelBuilder.Entity<ChiTietDangKy>()
                .HasOne(ct => ct.DangKy)
                .WithMany(d => d.ChiTietDangKys)
                .HasForeignKey(ct => ct.MaDK);

            modelBuilder.Entity<ChiTietDangKy>()
                .HasOne(ct => ct.HocPhan)
                .WithMany(h => h.ChiTietDangKys)
                .HasForeignKey(ct => ct.MaHP);

            // Seed data
            modelBuilder.Entity<NganhHoc>().HasData(
                new NganhHoc { MaNganh = "CNTT", TenNganh = "Công nghệ thông tin" },
                new NganhHoc { MaNganh = "QTKD", TenNganh = "Quản trị kinh doanh" }
            );

            modelBuilder.Entity<SinhVien>().HasData(
                new SinhVien
                {
                    MaSV = "2280600791",
                    HoTen = "Đặng Trí Hải",
                    GioiTinh = "Nam",
                    NgaySinh = new DateTime(2004, 10, 17),
                    Hinh = "/images/sv1.jpg",
                    MaNganh = "CNTT"
                },
                new SinhVien
                {
                    MaSV = "9876543210",
                    HoTen = "Nguyễn Thị B",
                    GioiTinh = "Nữ",
                    NgaySinh = new DateTime(2000, 3, 7),
                    Hinh = "/images/sv2.jpg",
                    MaNganh = "QTKD"
                }
            );

            modelBuilder.Entity<HocPhan>().HasData(
                new HocPhan { MaHP = "CNTT01", TenHP = "Lập trình C", SoTinChi = 3 },
                new HocPhan { MaHP = "CNTT02", TenHP = "Cơ sở dữ liệu", SoTinChi = 2 },
                new HocPhan { MaHP = "QTKD01", TenHP = "Kinh tế vi mô", SoTinChi = 2 },
                new HocPhan { MaHP = "QTDK02", TenHP = "Xác suất thống kê 1", SoTinChi = 3 }
            );
        }
    }
}