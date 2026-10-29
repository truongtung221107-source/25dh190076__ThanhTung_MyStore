using System.Linq;
using System.Web.Mvc;
using MyStore.Models.EDM;
using MyStore.Models.ViewModel;
using PagedList;
using System.Net;
namespace MyStore.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        public ActionResult Index(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            var products = db.Products.AsQueryable();

            // Tìm kiếm theo tên, mô tả hoặc tên danh mục
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.SearchTerm = searchTerm;
                products = products.Where(p => p.ProductName.Contains(searchTerm)
                                            || p.ProductDescription.Contains(searchTerm)
                                            || p.Category.CategoryName.Contains(searchTerm));
            }

            // Phân trang: 6 sản phẩm/trang
            int pageNumber = page ?? 1;
            int pageSize = 6;

            // 10 sản phẩm bán chạy nhất
            model.FeaturedProducts = products
                .OrderByDescending(p => p.OrderDetails.Count())
                .Take(10).ToList();

            // 20 sản phẩm bán ít nhất, có phân trang
            model.NewProducts = products
                .OrderBy(p => p.OrderDetails.Count())
                .Take(20)
                .ToPagedList(pageNumber, pageSize);

            return View(model);
        }
        public ActionResult ProductDetails(int? id, int? quantity, int? page)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product pro = db.Products.Find(id);
            if (pro == null)
                return HttpNotFound();

            // Các sản phẩm cùng danh mục (trừ sản phẩm đang xem)
            var sameCategory = db.Products
                .Where(p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID);

            var model = new ProductDetailVM();
            model.Product = pro;
            model.Quantity = quantity ?? 1;

            int pageNumber = page ?? 1;
            int pageSize = 3;

            model.RelatedProducts = sameCategory
                .OrderBy(p => p.ProductID)
                .Take(8).ToList();

            model.TopProducts = sameCategory
                .OrderByDescending(p => p.OrderDetails.Count())
                .Take(8)
                .ToPagedList(pageNumber, pageSize);

            return View(model);
        }
    }
}