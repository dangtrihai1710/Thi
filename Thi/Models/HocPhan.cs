﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thi.Models
{
    public class HocPhan
    {
        [Key]
        [Column(TypeName = "char(6)")]
        [Display(Name = "Mã học phần")]
        public string MaHP { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        [Display(Name = "Tên học phần")]
        public string TenHP { get; set; } = string.Empty;

        [Display(Name = "Số tín chỉ")]
        public int? SoTinChi { get; set; }

        [Display(Name = "Số lượng dự kiến")]
        public int? SoLuong { get; set; } = 100; // Mặc định 100 sinh viên

        // Navigation property
        public virtual ICollection<ChiTietDangKy> ChiTietDangKys { get; set; } = new List<ChiTietDangKy>();
    }
}