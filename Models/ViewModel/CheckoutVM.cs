using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyStore.Models.ViewModel
{
    public class CheckoutVM
    {
        // Thông tin khách hàng (chỉ để hiển thị)
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hình thức giao hàng")]
        [Display(Name = "Hình thức giao hàng")]
        public string DeliveryMethod { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hình thức thanh toán")]
        [Display(Name = "Hình thức thanh toán")]
        public string PaymentMethod { get; set; }

        // Các sản phẩm trong giỏ hàng
        public List<CartItem> Items { get; set; }

        // Tổng giá trị đơn hàng
        public decimal TotalAmount { get; set; }
    }
}