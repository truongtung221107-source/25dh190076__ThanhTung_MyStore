using System.Collections.Generic;
using MyStore.Models.EDM;

namespace MyStore.Models.ViewModel
{
    public class InventoryVM
    {
        // Hàng còn tồn kho (Quantity > 0)
        public List<Product> InStock { get; set; }

        // Hàng cần nhập thêm (Quantity = 0)
        public List<Product> OutOfStock { get; set; }
    }
}