using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thi.Data;
using Thi.Models;

namespace Thi.Controllers
{
    public class HocPhanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HocPhanController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Kiểm tra đăng nhập
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("IsLoggedIn")))
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để truy cập trang này.";
                return RedirectToAction("Login", "Auth");
            }

            var hocPhans = await _context.HocPhans.ToListAsync();

            // Lấy thông tin sinh viên đã đăng nhập
            var userID = HttpContext.Session.GetString("UserID");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserID = userID;

            // Lấy danh sách học phần đã đăng ký trong session (giỏ hàng)
            var selectedCourses = HttpContext.Session.GetString("SelectedCourses");
            if (!string.IsNullOrEmpty(selectedCourses))
            {
                ViewBag.SelectedCourses = selectedCourses.Split(',').ToList();
            }
            else
            {
                ViewBag.SelectedCourses = new List<string>();
            }

            return View(hocPhans);
        }

        [HttpPost]
        public IActionResult AddToCart(string maHP)
        {
            // Kiểm tra đăng nhập
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("IsLoggedIn")))
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thực hiện chức năng này." });
            }

            // Lấy danh sách học phần đã chọn từ session
            var selectedCourses = HttpContext.Session.GetString("SelectedCourses");
            var coursesList = string.IsNullOrEmpty(selectedCourses) ?
                new List<string>() : selectedCourses.Split(',').ToList();

            // Kiểm tra xem học phần đã được chọn chưa
            if (coursesList.Contains(maHP))
            {
                return Json(new { success = false, message = "Học phần này đã được thêm vào danh sách đăng ký." });
            }

            // Thêm học phần vào danh sách
            coursesList.Add(maHP);
            HttpContext.Session.SetString("SelectedCourses", string.Join(",", coursesList));

            return Json(new { success = true, message = "Đã thêm học phần vào danh sách đăng ký." });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(string maHP)
        {
            // Lấy danh sách học phần đã chọn từ session
            var selectedCourses = HttpContext.Session.GetString("SelectedCourses");
            if (!string.IsNullOrEmpty(selectedCourses))
            {
                var coursesList = selectedCourses.Split(',').ToList();
                coursesList.Remove(maHP);

                if (coursesList.Any())
                {
                    HttpContext.Session.SetString("SelectedCourses", string.Join(",", coursesList));
                }
                else
                {
                    HttpContext.Session.Remove("SelectedCourses");
                }
            }

            return Json(new { success = true, message = "Đã xóa học phần khỏi danh sách đăng ký." });
        }

        public async Task<IActionResult> Cart()
        {
            // Kiểm tra đăng nhập
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("IsLoggedIn")))
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để truy cập trang này.";
                return RedirectToAction("Login", "Auth");
            }

            var selectedCourses = HttpContext.Session.GetString("SelectedCourses");
            var hocPhans = new List<HocPhan>();

            if (!string.IsNullOrEmpty(selectedCourses))
            {
                var courseIds = selectedCourses.Split(',').ToList();
                hocPhans = await _context.HocPhans
                    .Where(h => courseIds.Contains(h.MaHP))
                    .ToListAsync();
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserID = HttpContext.Session.GetString("UserID");

            return View(hocPhans);
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("SelectedCourses");
            return Json(new { success = true, message = "Đã xóa tất cả học phần khỏi danh sách đăng ký." });
        }

        // Phương thức xóa từng học phần từ giỏ hàng (cho trang Cart)
        [HttpPost]
        public IActionResult RemoveFromCartPage(string maHP)
        {
            try
            {
                var selectedCourses = HttpContext.Session.GetString("SelectedCourses");
                if (!string.IsNullOrEmpty(selectedCourses))
                {
                    var coursesList = selectedCourses.Split(',').ToList();
                    coursesList.Remove(maHP);

                    if (coursesList.Any())
                    {
                        HttpContext.Session.SetString("SelectedCourses", string.Join(",", coursesList));
                    }
                    else
                    {
                        HttpContext.Session.Remove("SelectedCourses");
                    }
                }

                TempData["SuccessMessage"] = "Đã xóa học phần khỏi danh sách đăng ký.";
                return RedirectToAction("Cart");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa học phần.";
                return RedirectToAction("Cart");
            }
        }

        // Phương thức xóa tất cả học phần (Xóa đăng ký)
        [HttpPost]
        public IActionResult ClearAllRegistration()
        {
            try
            {
                HttpContext.Session.Remove("SelectedCourses");
                TempData["SuccessMessage"] = "Đã xóa tất cả học phần khỏi danh sách đăng ký.";
                return RedirectToAction("Cart");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa đăng ký.";
                return RedirectToAction("Cart");
            }
        }


        // Thêm vào cuối class HocPhanController
        [HttpPost]
        public async Task<IActionResult> SaveRegistration()
        {
            try
            {
                // Kiểm tra đăng nhập
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("IsLoggedIn")))
                {
                    return Json(new { success = false, message = "Bạn cần đăng nhập để thực hiện chức năng này." });
                }

                var userID = HttpContext.Session.GetString("UserID");
                var selectedCourses = HttpContext.Session.GetString("SelectedCourses");

                if (string.IsNullOrEmpty(selectedCourses))
                {
                    return Json(new { success = false, message = "Không có học phần nào được chọn để đăng ký." });
                }

                var courseIds = selectedCourses.Split(',').ToList();

                // Kiểm tra sinh viên đã có đăng ký chưa (một sinh viên chỉ được đăng ký một lần)
                var existingRegistration = await _context.DangKys
                    .FirstOrDefaultAsync(d => d.MaSV == userID);

                if (existingRegistration != null)
                {
                    return Json(new { success = false, message = "Bạn đã có đăng ký học phần trong hệ thống. Một sinh viên chỉ được đăng ký một lần." });
                }

                // Tạo bản ghi đăng ký mới
                var dangKy = new DangKy
                {
                    MaSV = userID,
                    NgayDK = DateTime.Now
                };

                _context.DangKys.Add(dangKy);
                await _context.SaveChangesAsync();

                // Lưu chi tiết đăng ký
                foreach (var courseId in courseIds)
                {
                    var chiTietDangKy = new ChiTietDangKy
                    {
                        MaDK = dangKy.MaDK,
                        MaHP = courseId
                    };
                    _context.ChiTietDangKys.Add(chiTietDangKy);
                }

                await _context.SaveChangesAsync();

                // Xóa session sau khi lưu thành công
                HttpContext.Session.Remove("SelectedCourses");

                return Json(new
                {
                    success = true,
                    message = $"Đăng ký thành công {courseIds.Count} học phần! Mã đăng ký: {dangKy.MaDK}",
                    registrationId = dangKy.MaDK
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra trong quá trình lưu đăng ký: " + ex.Message });
            }
        }

        // Thêm action để hiển thị thông tin đăng ký đã lưu
        public async Task<IActionResult> RegistrationResult(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userID = HttpContext.Session.GetString("UserID");
            var dangKy = await _context.DangKys
                .Include(d => d.SinhVien)
                .ThenInclude(s => s.NganhHoc)
                .Include(d => d.ChiTietDangKys)
                .ThenInclude(ct => ct.HocPhan)
                .FirstOrDefaultAsync(d => d.MaDK == id && d.MaSV == userID);

            if (dangKy == null)
            {
                return NotFound();
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserID = userID;

            return View(dangKy);
        }

        // Thêm action để xem tất cả đăng ký của sinh viên
        public async Task<IActionResult> MyRegistrations()
        {
            // Kiểm tra đăng nhập
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("IsLoggedIn")))
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để truy cập trang này.";
                return RedirectToAction("Login", "Auth");
            }

            var userID = HttpContext.Session.GetString("UserID");
            var dangKys = await _context.DangKys
                .Include(d => d.SinhVien)
                .ThenInclude(s => s.NganhHoc)
                .Include(d => d.ChiTietDangKys)
                .ThenInclude(ct => ct.HocPhan)
                .Where(d => d.MaSV == userID)
                .OrderByDescending(d => d.NgayDK)
                .ToListAsync();

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserID = userID;

            return View(dangKys);
        }
    }

}