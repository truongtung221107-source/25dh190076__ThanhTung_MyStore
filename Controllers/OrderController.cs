using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MyStore.Models.EDM;
using MyStore.Models.ViewModel;

namespace MyStore.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // Lấy khách hàng đang đăng nhập (chưa đăng nhập thì trả về null)
        private Customer GetCustomer()
        {
            if (!User.Identity.IsAuthenticated)
                return null;

            string username = User.Identity.Name;
            return db.Customers.FirstOrDefault(c => c.Username == username);
        }

        // TRANG THANH TOÁN
        [HttpGet]
        public ActionResult Checkout()
        {
            var customer = GetCustomer();
            if (customer == null)
                return RedirectToAction("Login", "Account");

            var cart = new CartService(Session).GetCart();
            if (!cart.Items.Any())
                return RedirectToAction("Index", "Home");

            var model = new CheckoutVM
            {
                CustomerName = customer.CustomerName,
                CustomerPhone = customer.CustomerPhone,
                ShippingAddress = customer.CustomerAddress,   // địa chỉ mặc định khi đăng ký
                Items = cart.Items.ToList(),
                TotalAmount = cart.TotalValue()
            };
            return View(model);
        }

        // ĐẶT HÀNG
        [HttpPost]
        public ActionResult Checkout(CheckoutVM model)
        {
            var customer = GetCustomer();
            if (customer == null)
                return RedirectToAction("Login", "Account");

            var cartService = new CartService(Session);
            var cart = cartService.GetCart();
            if (!cart.Items.Any())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                // Nạp lại dữ liệu hiển thị rồi trả về form
                model.CustomerName = customer.CustomerName;
                model.CustomerPhone = customer.CustomerPhone;
                model.Items = cart.Items.ToList();
                model.TotalAmount = cart.TotalValue();
                return View(model);
            }

            // Tạo đơn hàng
            var order = new Order
            {
                CustomerID = customer.CustomerID,
                OrderDate = DateTime.Now,
                TotalAmount = cart.TotalValue(),
                PaymentStatus = "Chờ thanh toán",
                ShippingAddress = model.ShippingAddress,
                DeliveryMethod = model.DeliveryMethod,
                PaymentMethod = model.PaymentMethod
            };

            // Thêm chi tiết đơn hàng từ giỏ
            foreach (var item in cart.Items)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            db.Orders.Add(order);
            db.SaveChanges();

            // Xóa giỏ hàng sau khi đặt xong
            cartService.ClearCart();

            return RedirectToAction("OrderSuccess", new { id = order.OrderID });
        }

        // TRANG XÁC NHẬN ĐƠN HÀNG
        public ActionResult OrderSuccess(int? id)
        {
            var customer = GetCustomer();
            if (customer == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
                return RedirectToAction("Index", "Home");

            var order = db.Orders
                .Include("OrderDetails.Product")
                .FirstOrDefault(o => o.OrderID == id && o.CustomerID == customer.CustomerID);

            if (order == null)
                return HttpNotFound();

            return View(order);
        }
        // CHI TIẾT ĐƠN HÀNG (hóa đơn)
        public ActionResult OrderDetail(int? id)
        {
            var customer = GetCustomer();
            if (customer == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
                return RedirectToAction("MyOrder");

            // Chỉ cho xem đơn của chính khách hàng đang đăng nhập
            var order = db.Orders
                .Include("Customer")
                .Include("OrderDetails.Product")
                .FirstOrDefault(o => o.OrderID == id && o.CustomerID == customer.CustomerID);

            if (order == null)
                return HttpNotFound();

            return View(order);
        }
        // TRANG ĐƠN HÀNG CỦA TÔI
        public ActionResult MyOrder(string status, string searchTerm)
        {
            var customer = GetCustomer();
            if (customer == null)
                return RedirectToAction("Login", "Account");

            var orders = db.Orders
                .Include("OrderDetails.Product")
                .Where(o => o.CustomerID == customer.CustomerID);

            // Lọc theo trạng thái (tab)
            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(o => o.PaymentStatus == status);
            }

            // Tìm theo mã đơn hàng hoặc tên sản phẩm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                int orderId;
                bool isId = int.TryParse(searchTerm, out orderId);
                orders = orders.Where(o => (isId && o.OrderID == orderId)
                                        || o.OrderDetails.Any(d => d.Product.ProductName.Contains(searchTerm)));
            }

            ViewBag.Status = status;
            ViewBag.SearchTerm = searchTerm;

            var list = orders
                .OrderByDescending(o => o.OrderDate)
                .ThenByDescending(o => o.OrderID)
                .ToList();

            return View(list);
        }
    }
}