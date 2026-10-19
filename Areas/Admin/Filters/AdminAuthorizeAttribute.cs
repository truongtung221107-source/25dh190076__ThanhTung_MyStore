using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

// (giữ nguyên dòng namespace do Visual Studio tạo, chỉ thay phần bên trong)
public class AdminAuthorizeAttribute : AuthorizeAttribute
{
    // Cho qua khi đã đăng nhập (key phải trùng với layout và AuthController)
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        return httpContext.Session["Admin"] != null;
    }

    // Chưa đăng nhập thì chuyển về trang Login của Admin
    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary(new { area = "Admin", controller = "Auth", action = "Login" }));
    }
}