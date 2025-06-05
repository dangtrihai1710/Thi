using System.ComponentModel.DataAnnotations;

namespace Thi.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Mã sinh viên là bắt buộc")]
        [Display(Name = "Mã sinh viên")]
        public string MaSV { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; } = string.Empty;

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; }
    }
}