using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MyStore.Models.EDM;
using PagedList;

namespace MyStore.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class OrdersController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Orders
        public ActionResult Index(string searchString, int? page)
        {
            var orders = db.Orders.Include(o => o.Customer);

            // Tìm theo mã đơn hàng hoặc tên khách hàng
            if (!string.IsNullOrEmpty(searchString))
            {
                int orderId;
                bool isId = int.TryParse(searchString, out orderId);
                orders = orders.Where(o => (isId && o.OrderID == orderId)
                                        || o.Customer.CustomerName.Contains(searchString));
            }

            ViewBag.SearchString = searchString;

            return View(orders.OrderByDescending(o => o.OrderID).ToPagedList(page ?? 1, 5));
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = db.Orders
                .Include(o => o.Customer)
                .Include("OrderDetails.Product")
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}