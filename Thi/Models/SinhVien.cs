using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thi.Models
{
    public class SinhVien
    {
        [Key]
        [Column(TypeName = "char(10)")]
        [Display(Name = "Mã sinh viên")]
        public string MaSV { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; } = string.Empty;

        [MaxLength(5)]
        [Display(Name = "Giới tính")]
        public string? GioiTinh { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [MaxLength(50)]
        [Display(Name = "Hình ảnh")]
        public string? Hinh { get; set; }

        [Column(TypeName = "char(4)")]
        [Display(Name = "Ngành học")]
        public string? MaNganh { get; set; }

        // Navigation properties
        [ForeignKey("MaNganh")]
        public virtual NganhHoc? NganhHoc { get; set; }
        public virtual ICollection<DangKy> DangKys { get; set; } = new List<DangKy>();
    }
}