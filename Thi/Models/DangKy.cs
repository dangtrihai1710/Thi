using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thi.Models
{
    public class DangKy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaDK { get; set; }

        [Display(Name = "Ngày đăng ký")]
        [DataType(DataType.Date)]
        public DateTime? NgayDK { get; set; }

        [Column(TypeName = "char(10)")]
        [Display(Name = "Mã sinh viên")]
        public string? MaSV { get; set; }

        // Navigation properties
        [ForeignKey("MaSV")]
        public virtual SinhVien? SinhVien { get; set; }
        public virtual ICollection<ChiTietDangKy> ChiTietDangKys { get; set; } = new List<ChiTietDangKy>();
    }
}