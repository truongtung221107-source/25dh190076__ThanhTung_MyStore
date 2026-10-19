using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MyStore.Models.EDM;

namespace MyStore.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            // 1. Các ô tổng quan
            ViewBag.TotalProducts = db.Products.Count();
            ViewBag.TotalCustomers = db.Customers.Count();
            ViewBag.TotalOrders = db.Orders.Count();

            // 2. Doanh thu theo tháng (tính từ Order.TotalAmount)
            var revenue = db.Orders.ToList()
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new object[] { g.Key.Month + "/" + g.Key.Year, g.Sum(o => o.TotalAmount) })
                .ToList();
            ViewBag.RevenueData = revenue;
            ViewBag.TotalRevenue = db.Orders.Any() ? db.Orders.Sum(o => o.TotalAmount) : 0m;

            // 3. Số sản phẩm theo danh mục
            var byCategory = db.Products.Include(p => p.Category).ToList()
                .GroupBy(p => p.Category != null ? p.Category.CategoryName : "Chưa phân loại")
                .Select(g => new object[] { g.Key, g.Count() })
                .ToList();
            ViewBag.CategoryData = byCategory;

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}