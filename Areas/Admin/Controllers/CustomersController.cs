using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MyStore.Models.EDM;
using PagedList;

namespace MyStore.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class CustomersController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Customers
        public ActionResult Index(string searchString, string sortOrder, int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            var customers = db.Customers.AsQueryable();

            // Tìm theo tên, điện thoại, email hoặc tên đăng nhập
            if (!string.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.CustomerName.Contains(searchString)
                                              || c.CustomerPhone.Contains(searchString)
                                              || c.CustomerEmail.Contains(searchString)
                                              || c.Username.Contains(searchString));
            }

            // Sắp xếp
            switch (sortOrder)
            {
                case "name_asc":
                    customers = customers.OrderBy(c => c.CustomerName);
                    break;
                case "name_desc":
                    customers = customers.OrderByDescending(c => c.CustomerName);
                    break;
                default:
                    customers = customers.OrderBy(c => c.CustomerID);
                    break;
            }

            ViewBag.SearchString = searchString;
            ViewBag.SortOrder = sortOrder;

            return View(customers.ToPagedList(pageNum, pageSize));
        }

        // GET: Admin/Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Admin/Customers/OrderDetails/5  (xem chi tiết một đơn hàng)
        public ActionResult OrderDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails.Select(d => d.Product))
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