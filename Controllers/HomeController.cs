using System.Linq;
using System.Web.Mvc;
using MyStore.Models.EDM; // Thay MyStore bằng tên namespace project của bạn nếu khác

public class HomeController : Controller
{
    private MyStoreEntities db = new MyStoreEntities(); // Khởi tạo kết nối database

    public ActionResult Index(string searchString)
    {
        var products = db.Products.AsQueryable();

        // Tích hợp luôn tính năng tìm kiếm đơn giản ngoài trang chủ cho khách
        if (!string.IsNullOrEmpty(searchString))
        {
            products = products.Where(p => p.ProductName.Contains(searchString));
            ViewBag.SearchString = searchString;
        }

        return View(products.ToList());
    }

    public ActionResult About()
    {
        ViewBag.Message = "Your application description page.";
        return View();
    }

    public ActionResult Contact()
    {
        ViewBag.Message = "Your contact page.";
        return View();
    }
    // Xem chi tiết sản phẩm cho khách hàng
    public ActionResult Details(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        var product = db.Products.Find(id);
        if (product == null)
        {
            return HttpNotFound();
        }

        return View(product);
    }
}