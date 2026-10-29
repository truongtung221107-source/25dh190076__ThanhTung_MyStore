using System.Web;

namespace MyStore.Models.ViewModel
{
    public class CartService
    {
        private readonly HttpSessionStateBase session;

        public CartService(HttpSessionStateBase session)
        {
            this.session = session;
        }

        // Lấy giỏ hàng từ Session (chưa có thì tạo mới)
        public Cart GetCart()
        {
            var cart = (Cart)session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                session["Cart"] = cart;
            }
            return cart;
        }

        public void ClearCart()
        {
            session["Cart"] = null;
        }
    }
}