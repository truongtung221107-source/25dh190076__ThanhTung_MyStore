using MyStore.Models.EDM;
using PagedList;

namespace MyStore.Models.ViewModel
{
    public class ProductListVM
    {
        // Danh mục đang xem
        public Category Category { get; set; }

        // Tiêu chí tìm kiếm, lọc, sắp xếp
        public string SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SortOrder { get; set; }

        // Danh sách sản phẩm đã phân trang
        public IPagedList<Product> Products { get; set; }
    }
}