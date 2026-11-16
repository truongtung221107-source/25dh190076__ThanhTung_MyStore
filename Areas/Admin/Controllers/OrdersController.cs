using MyStore.Models.EDM;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyStore.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class OrdersController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Orders
        public ActionResult Index(string searchTerm, string paymentStatus, DateTime? orderDate, int? page)
        {
            var orders = db.Orders.Include(o => o.Customer).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string keyword = searchTerm.Trim();
                orders = orders.Where(o => o.OrderID.ToString().Contains(keyword)
                                        || o.Customer.CustomerName.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(paymentStatus))
            {
                orders = orders.Where(o => o.PaymentStatus == paymentStatus);
            }

            if (orderDate.HasValue)
            {
                DateTime selectedDate = orderDate.Value.Date;
                orders = orders.Where(o => DbFunctions.TruncateTime(o.OrderDate) == selectedDate);
            }

            var statuses = new List<string> { "Chờ thanh toán", "Đang xử lý", "Đang vận chuyển", "Đã giao", "Đã hủy" };
            ViewBag.PaymentStatusList = new SelectList(statuses, paymentStatus);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.PaymentStatus = paymentStatus;
            ViewBag.OrderDate = orderDate.HasValue ? orderDate.Value.ToString("yyyy-MM-dd") : "";

            orders = orders.OrderByDescending(o => o.OrderDate).ThenByDescending(o => o.OrderID);

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(orders.ToPagedList(pageNumber, pageSize));
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
        // GET: Admin/Orders/UpdateStatus/5
        public ActionResult UpdateStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            var statuses = new List<string> { "Chờ thanh toán", "Đang xử lý", "Đang vận chuyển", "Đã giao", "Đã hủy" };
            ViewBag.StatusList = new SelectList(statuses, order.PaymentStatus);

            return View(order);
        }

        // POST: Admin/Orders/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int id, string PaymentStatus)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            order.PaymentStatus = PaymentStatus;
            db.SaveChanges();

            return RedirectToAction("Details", new { id = order.OrderID });
        }
    }
}