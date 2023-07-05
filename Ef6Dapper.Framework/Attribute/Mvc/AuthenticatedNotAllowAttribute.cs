using System.Web.Mvc;
using System.Web.Routing;

namespace OnePage.Framework.Attribute.Mvc
{
    /// <summary>
    /// 不允許已登入使用者，若已登入導向至後台。
    /// </summary>
    public class AuthenticatedNotAllowAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 在進行授權時呼叫。
        /// </summary>
        /// <param name="filterContext">封裝使用 System.Web.Mvc.AuthorizeAttribute 屬性所需的資訊。</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
                filterContext.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary
                                   {
                                       { "action", "Index" },
                                       { "controller", "Default" },
                                       { "area", "Admin" }
                                   });
        }
    }
}
