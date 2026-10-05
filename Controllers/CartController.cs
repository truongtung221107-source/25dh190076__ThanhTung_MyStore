using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyStore.Models;
using MyStore.Models.EDM;

namespace MyStore.Controllers
{
    public class CartController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        public List<CartItem> GetCart()
        {
            var listCart = Session["Cart"] as List<CartItem>;
            if (listCart == null)
            {
                listCart = new List<CartItem>();
                Session["Cart"] = listCart;
            }
            return listCart;
        }

        public ActionResult AddToCart(int id, int quantity = 1)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(p => p.ProductID == id);

            if (item == null)
            {
                var product = db.Products.Find(id);
                if (product != null)
                {
                    cart.Add(new CartItem
                    {
                        ProductID = product.ProductID,
                        ProductName = product.ProductName,
                        ProductImage = product.ProductImage,
                        ProductPrice = product.ProductPrice,
                        Quantity = quantity
                    });
                }
            }
            else
            {
                item.Quantity += quantity;
            }

            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            var cart = GetCart();
            ViewBag.Total = cart.Sum(p => p.TotalPrice);
            return View(cart);
        }

        public ActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(p => p.ProductID == id);
            if (item != null)
            {
                cart.Remove(item);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Checkout()
        {
            var cart = GetCart();
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index");
            }
            ViewBag.Total = cart.Sum(p => p.TotalPrice);
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(string customerName, string customerPhone, string customerAddress)
        {
            var cart = GetCart();
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index");
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var order = new Order
                    {
                        CustomerID = 1,
                        OrderDate = System.DateTime.Now,
                        TotalAmount = cart.Sum(p => p.TotalPrice),
                        PaymentStatus = "Chưa thanh toán",
                        AddressDelivery = customerAddress + " - Người nhận: " + customerName + " (" + customerPhone + ")"
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();

                    foreach (var item in cart)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderID = order.OrderID,
                            ProductID = item.ProductID,
                            Quantity = item.Quantity,
                            // Nếu trong OrderDetail của bạn cột giá tên khác, ví dụ UnitPrice hoặc ProductPrice thì gõ lại ở đây:
                            UnitPrice = (decimal)item.ProductPrice
                        };
                        db.OrderDetails.Add(orderDetail);
                    }
                    db.SaveChanges();

                    transaction.Commit();
                    Session["Cart"] = null;

                    return RedirectToAction("OrderSuccess");
                }
                catch
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "Có lỗi xảy ra trong quá trình đặt hàng. Vui lòng thử lại!");
                }
            }

            ViewBag.Total = cart.Sum(p => p.TotalPrice);
            return View();
        }

        public ActionResult OrderSuccess()
        {
            return View();
        }
    }
}