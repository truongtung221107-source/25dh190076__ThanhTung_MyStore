using System.ComponentModel.DataAnnotations;

namespace MyStore.Models.EDM
{
    public class CategoryMetadata
    {
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tên từ 2 đến 100 ký tự")]
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; }
    }

    public class ProductMetadata
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200, ErrorMessage = "Tên tối đa 200 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        [Display(Name = "Mô tả")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(1, 1000000000, ErrorMessage = "Giá phải lớn hơn 0")]
        [Display(Name = "Giá")]
        public decimal ProductPrice { get; set; }
    }

    public class UserMetadata
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Tên đăng nhập từ 3 ký tự")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 đến 50 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vai trò không được để trống")]
        [Display(Name = "Vai trò")]
        public string UserRole { get; set; }
    }

    public class CustomerMetadata
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [Display(Name = "Họ tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string CustomerEmail { get; set; }

        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }
    }

    public class OrderMetadata
    {
        [DataType(DataType.Date)]
        [Display(Name = "Ngày đặt")]
        public System.DateTime OrderDate { get; set; }

        [Range(0, 1000000000, ErrorMessage = "Tổng tiền không hợp lệ")]
        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Địa chỉ giao hàng không được để trống")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string AddressDelivery { get; set; }
    }

    public class OrderDetailMetadata
    {
        [Range(1, 1000, ErrorMessage = "Số lượng từ 1 đến 1000")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Range(0, 1000000000, ErrorMessage = "Đơn giá không hợp lệ")]
        [Display(Name = "Đơn giá")]
        public decimal UnitPrice { get; set; }
    }
}