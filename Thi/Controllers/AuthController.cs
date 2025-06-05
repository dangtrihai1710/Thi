using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thi.Data;
using Thi.Models;

namespace Thi.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra sinh viên có tồn tại không
                var sinhVien = await _context.SinhViens
                    .Include(s => s.NganhHoc)
                    .FirstOrDefaultAsync(s => s.MaSV == model.MaSV);

                if (sinhVien != null)
                {
                    // Lưu thông tin đăng nhập vào session
                    HttpContext.Session.SetString("UserID", sinhVien.MaSV);
                    HttpContext.Session.SetString("UserName", sinhVien.HoTen);
                    HttpContext.Session.SetString("IsLoggedIn", "true");

                    TempData["SuccessMessage"] = $"Xin chào {sinhVien.HoTen}! Đăng nhập thành công.";
                    return RedirectToAction("Index", "HocPhan");
                }
                else
                {
                    ModelState.AddModelError("", "Mã sinh viên không tồn tại trong hệ thống.");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Đăng xuất thành công.";
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}