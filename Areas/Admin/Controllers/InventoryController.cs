using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MyStore.Models.EDM;
using MyStore.Models.ViewModel;

namespace MyStore.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class InventoryController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Inventory
        public ActionResult Index()
        {
            var model = new InventoryVM
            {
                InStock = db.Products.Include(p => p.Category)
                            .Where(p => p.Quantity > 0)
                            .OrderBy(p => p.ProductName).ToList(),

                OutOfStock = db.Products.Include(p => p.Category)
                            .Where(p => p.Quantity == 0)
                            .OrderBy(p => p.ProductName).ToList()
            };
            return View(model);
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