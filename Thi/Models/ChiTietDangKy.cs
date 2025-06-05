using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thi.Models
{
    public class ChiTietDangKy
    {
        public int MaDK { get; set; }

        [Column(TypeName = "char(6)")]
        public string MaHP { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("MaDK")]
        public virtual DangKy? DangKy { get; set; }

        [ForeignKey("MaHP")]
        public virtual HocPhan? HocPhan { get; set; }
    }
}