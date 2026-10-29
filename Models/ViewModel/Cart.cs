using System.Collections.Generic;
using System.Linq;

namespace MyStore.Models.ViewModel
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        public IEnumerable<CartItem> Items => items;

        // Thêm sản phẩm vào giỏ (nếu đã có thì cộng dồn số lượng)
        public void AddItem(int productId, string productImage, string productName,
                            decimal unitPrice, int quantity, string category)
        {
            var existing = items.FirstOrDefault(i => i.ProductID == productId);
            if (existing == null)
            {
                items.Add(new CartItem
                {
                    ProductID = productId,
                    ProductImage = productImage,
                    ProductName = productName,
                    UnitPrice = unitPrice,
                    Quantity = quantity,
                    Category = category
                });
            }
            else
            {
                existing.Quantity += quantity;
            }
        }

        // Xóa sản phẩm khỏi giỏ
        public void RemoveItem(int productId)
        {
            items.RemoveAll(i => i.ProductID == productId);
        }

        // Tổng giá trị giỏ hàng
        public decimal TotalValue()
        {
            return items.Sum(i => i.TotalPrice);
        }

        // Làm trống giỏ
        public void Clear()
        {
            items.Clear();
        }

        // Cập nhật số lượng
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = items.FirstOrDefault(i => i.ProductID == productId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
        }
    }
}