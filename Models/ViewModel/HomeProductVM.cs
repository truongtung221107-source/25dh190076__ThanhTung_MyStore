using System.Collections.Generic;
using PagedList;
using MyStore.Models.EDM;

namespace MyStore.Models.ViewModel
{
    public class HomeProductVM
    {
        // Từ khóa tìm kiếm
        public string SearchTerm { get; set; }

        // Hỗ trợ phân trang
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;

        // 10 sản phẩm bán chạy nhất (slider)
        public List<Product> FeaturedProducts { get; set; }

        // 20 sản phẩm mới, đã phân trang
        public IPagedList<Product> NewProducts { get; set; }
    }
}