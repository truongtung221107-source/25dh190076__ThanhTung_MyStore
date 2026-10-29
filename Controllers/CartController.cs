using System;
using System.Web.Mvc;
using MyStore.Models.EDM;
using MyStore.Models.ViewModel;

namespace MyStore.Controllers
{
    public class CartController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // Lấy dịch vụ giỏ hàng (lưu trong Session)
        private CartService GetCartService()
        {
            return new CartService(Session);
        }

        // Hiển thị giỏ hàng
        public ActionResult Index()
        {
            var cart = GetCartService().GetCart();
            return View(cart);
        }

        // Thêm sản phẩm vào giỏ
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity = 1)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            var cart = GetCartService().GetCart();
            cart.AddItem(
                product.ProductID,
                product.ProductImage,
                product.ProductName,
                Convert.ToDecimal(product.ProductPrice),
                quantity,
                product.Category.CategoryName);

            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            GetCartService().GetCart().RemoveItem(id);
            return RedirectToAction("Index");
        }

        // Làm trống giỏ hàng
        public ActionResult ClearCart()
        {
            GetCartService().ClearCart();
            return RedirectToAction("Index");
        }

        // Cập nhật số lượng
        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            if (quantity < 1) quantity = 1;
            GetCartService().GetCart().UpdateQuantity(id, quantity);
            return RedirectToAction("Index");
        }
    }
}