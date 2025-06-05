using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thi.Models
{
    public class SinhVien
    {
        [Key]
        [Column(TypeName = "char(10)")]
        [Display(Name = "Mã sinh viên")]
        [Required(ErrorMessage = "Mã sinh viên là bắt buộc")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mã sinh viên phải có đúng 10 ký tự")]
        public string MaSV { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [MaxLength(50)]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; } = string.Empty;

        [MaxLength(5)]
        [Display(Name = "Giới tính")]
        public string? GioiTinh { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [MaxLength(200)] // Tăng từ 50 lên 200 để chứa đường dẫn file dài hơn
        [Display(Name = "Hình ảnh")]
        public string? Hinh { get; set; }

        [Column(TypeName = "char(4)")]
        [Display(Name = "Ngành học")]
        [Required(ErrorMessage = "Ngành học là bắt buộc")]
        public string? MaNganh { get; set; }

        // Navigation properties
        [ForeignKey("MaNganh")]
        public virtual NganhHoc? NganhHoc { get; set; }
        public virtual ICollection<DangKy> DangKys { get; set; } = new List<DangKy>();
    }
}