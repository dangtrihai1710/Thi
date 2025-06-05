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
    }
}