namespace MyStore.Models
{
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; } // Số lượng mua
        public decimal TotalPrice => ProductPrice * Quantity; // Thành tiền
    }
}