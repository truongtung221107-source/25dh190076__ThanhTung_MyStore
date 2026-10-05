using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace MyStore.Models.EDM
{
    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
    }

    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
        // Thuộc tính tự thêm, dùng để upload ảnh sản phẩm, không tạo cột trong database
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }

    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
    }

    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    {
    }

    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
    }

    [MetadataType(typeof(OrderDetailMetadata))]
    public partial class OrderDetail
    {
    }
}