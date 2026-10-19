using System.Linq;
using System.Web.Mvc;
using MyStore.Models.EDM;

namespace MyStore.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Auth/Login
        public ActionResult Login()
        {
            // Nếu đã đăng nhập rồi thì tự động chuyển vào trang danh sách Tài khoản
            if (Session["Admin"] != null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }

        // POST: Admin/Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            // Kiểm tra rỗng
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!";
                return View();
            }

            // Tìm tài khoản trong DB (Chỉ cho phép tài khoản có Role = "A" (Admin) đăng nhập)
            var admin = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password && u.UserRole == "A");

            if (admin != null)
            {
                // Đăng nhập thành công -> Lưu thông tin vào Session
                Session["Admin"] = admin;
                return RedirectToAction("Index", "Users"); // Chuyển hướng tới trang quản lý Tài khoản
            }
            else
            {
                // Đăng nhập thất bại
                ViewBag.Error = "Tên đăng nhập, mật khẩu không đúng hoặc bạn không có quyền Admin!";
                return View();
            }
        }

        // GET: Admin/Auth/Logout
        public ActionResult Logout()
        {
            // Xóa Session và quay về trang Đăng nhập
            Session.Remove("Admin");
            return RedirectToAction("Login");
        }
    }
}