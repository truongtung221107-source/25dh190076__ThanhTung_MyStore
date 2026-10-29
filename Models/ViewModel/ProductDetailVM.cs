using System.Collections.Generic;
using MyStore.Models.EDM;
using PagedList;

namespace MyStore.Models.ViewModel
{
    public class ProductDetailVM
    {
        // Sản phẩm đang xem
        public Product Product { get; set; }

        // Số lượng đặt mua
        public int Quantity { get; set; } = 1;

        // 8 sản phẩm cùng danh mục (slider)
        public List<Product> RelatedProducts { get; set; }

        // Sản phẩm bán chạy cùng danh mục, phân trang 3 sản phẩm/trang
        public IPagedList<Product> TopProducts { get; set; }
    }
}