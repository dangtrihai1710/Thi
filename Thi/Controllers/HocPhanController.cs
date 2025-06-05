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
        public async Task<IActionResult> AddToCart(string maHP)
        {
            // Kiểm tra đăng nhập
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("IsLoggedIn")))
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thực hiện chức năng này." });
            }

            // Kiểm tra số lượng còn lại
            var hocPhan = await _context.HocPhans.FindAsync(maHP);
            if (hocPhan == null)
            {
                return Json(new { success = false, message = "Học phần không tồn tại." });
            }

            if (hocPhan.SoLuong <= 0)
            {
                return Json(new { success = false, message = "Học phần đã hết chỗ." });
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

        [HttpPost]
        public async Task<IActionResult> SaveRegistration()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
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

                // Kiểm tra sinh viên đã có đăng ký chưa
                var existingRegistration = await _context.DangKys
                    .FirstOrDefaultAsync(d => d.MaSV == userID);

                if (existingRegistration != null)
                {
                    return Json(new { success = false, message = "Bạn đã có đăng ký học phần trong hệ thống. Một sinh viên chỉ được đăng ký một lần." });
                }

                // Kiểm tra số lượng của từng học phần trước khi lưu
                foreach (var courseId in courseIds)
                {
                    var hocPhan = await _context.HocPhans.FindAsync(courseId);
                    if (hocPhan == null)
                    {
                        await transaction.RollbackAsync();
                        return Json(new { success = false, message = $"Học phần {courseId} không tồn tại." });
                    }

                    if (hocPhan.SoLuong <= 0)
                    {
                        await transaction.RollbackAsync();
                        return Json(new { success = false, message = $"Học phần {hocPhan.TenHP} đã hết chỗ." });
                    }
                }

                // Tạo bản ghi đăng ký mới
                var dangKy = new DangKy
                {
                    MaSV = userID,
                    NgayDK = DateTime.Now
                };

                _context.DangKys.Add(dangKy);
                await _context.SaveChangesAsync();

                // Lưu chi tiết đăng ký và cập nhật số lượng
                foreach (var courseId in courseIds)
                {
                    // Thêm chi tiết đăng ký
                    var chiTietDangKy = new ChiTietDangKy
                    {
                        MaDK = dangKy.MaDK,
                        MaHP = courseId
                    };
                    _context.ChiTietDangKys.Add(chiTietDangKy);

                    // Giảm số lượng học phần
                    var hocPhan = await _context.HocPhans.FindAsync(courseId);
                    if (hocPhan != null && hocPhan.SoLuong > 0)
                    {
                        hocPhan.SoLuong--;
                        _context.HocPhans.Update(hocPhan);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Xóa session sau khi lưu thành công
                HttpContext.Session.Remove("SelectedCourses");

                return Json(new
                {
                    success = true,
                    message = $"Đăng ký thành công {courseIds.Count} học phần! Mã đăng ký: {dangKy.MaDK}. Số lượng học phần đã được cập nhật.",
                    registrationId = dangKy.MaDK
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "Có lỗi xảy ra trong quá trình lưu đăng ký: " + ex.Message });
            }
        }

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

        // Thêm action để quản lý học phần (dành cho admin)
        public async Task<IActionResult> ManageHocPhan()
        {
            var hocPhans = await _context.HocPhans.ToListAsync();
            return View(hocPhans);
        }

        // Action để reset số lượng học phần (dành cho admin)
        [HttpPost]
        public async Task<IActionResult> ResetSoLuong(string maHP, int soLuong)
        {
            try
            {
                var hocPhan = await _context.HocPhans.FindAsync(maHP);
                if (hocPhan != null)
                {
                    hocPhan.SoLuong = soLuong;
                    _context.HocPhans.Update(hocPhan);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = $"Đã cập nhật số lượng học phần {hocPhan.TenHP} thành {soLuong}" });
                }
                return Json(new { success = false, message = "Không tìm thấy học phần" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}