using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thi.Models
{
    public class NganhHoc
    {
        [Key]
        [Column(TypeName = "char(4)")]
        public string MaNganh { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string TenNganh { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<SinhVien> SinhViens { get; set; } = new List<SinhVien>();
    }
}