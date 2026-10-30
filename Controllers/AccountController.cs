using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MyStore.Models.EDM;
using MyStore.Models.ViewModel;

namespace MyStore.Controllers
{
    public class AccountController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // Lưu thông tin đăng nhập của khách hàng vào Session và Cookie
        private void SignIn(string username, int customerId, string customerName, bool remember)
        {
            Session["CustomerUsername"] = username;
            Session["CustomerID"] = customerId;
            Session["CustomerName"] = customerName;
            FormsAuthentication.SetAuthCookie(username, remember);
        }

        // ĐĂNG KÝ
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Kiểm tra tên đăng nhập đã tồn tại chưa
            if (db.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                return View(model);
            }

            // 01 bản ghi trong bảng User
            var user = new User
            {
                Username = model.Username,
                Password = model.Password,
                UserRole = "C"
            };

            // 01 bản ghi trong bảng Customer
            var customer = new Customer
            {
                CustomerName = model.CustomerName,
                CustomerPhone = model.CustomerPhone,
                CustomerEmail = model.CustomerEmail,
                CustomerAddress = model.CustomerAddress,
                Username = model.Username
            };

            db.Users.Add(user);
            db.Customers.Add(customer);
            db.SaveChanges();

            // Đăng ký xong thì đăng nhập luôn và về Trang chủ
            SignIn(user.Username, customer.CustomerID, customer.CustomerName, false);
            return RedirectToAction("Index", "Home");
        }

        // ĐĂNG NHẬP
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = db.Users.FirstOrDefault(u => u.Username == model.Username);

            // Password là NCHAR(50) nên phải Trim() khi so sánh
            if (user != null && user.Password.Trim() == model.Password && user.UserRole.Trim() == "C")
            {
                var customer = db.Customers.FirstOrDefault(c => c.Username == user.Username);
                if (customer != null)
                {
                    SignIn(user.Username, customer.CustomerID, customer.CustomerName, model.RememberMe);
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            return View(model);
        }

        // ĐĂNG XUẤT
        public ActionResult Logout()
        {
            Session.Remove("CustomerUsername");
            Session.Remove("CustomerID");
            Session.Remove("CustomerName");
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}