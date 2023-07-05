using System.Net;
using System.Web;
using System.Web.Mvc;
using OnePage.Data;
using OnePage.Service.Interface;

namespace OnePage.Framework.Xsrf
{
    /// <summary>
    /// AJAX防範偽造要求的屬性。
    /// </summary>
    public class AjaxValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly ILogService logService;

        public AjaxValidateAntiForgeryTokenAttribute()
        {
            Order = 2;
            logService = DependencyResolver.Current.GetService<ILogService>();
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAjaxValidateAntiForgeryTokenAttribute), inherit: true)
                        || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAjaxValidateAntiForgeryTokenAttribute), inherit: true);

            if (skipAuthorization)
                return;

            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                HandleInvalidRequest(filterContext);
                return;
            }

            string token = filterContext.HttpContext.Request.Headers[AntiForgeryUtility.XsrfHeaderKey];

            HttpCookie cookie = filterContext.HttpContext.Request.Cookies[AntiForgeryUtility.XsrfCookieName];

            if (cookie == null)
            {
                HandleInvalidRequest(filterContext);
                return;
            }

            if (token != cookie.Value)
            {
                HandleInvalidRequest(filterContext);
                return;
            }

            if (!AntiForgeryUtility.ValidateToken(token))
            {
                HandleInvalidRequest(filterContext);
                return;
            }
        }

        /// <summary>
        /// 處理無效的需求。
        /// </summary>
        /// <param name="filterContext">封裝使用 System.Web.Mvc.AuthorizeAttribute 屬性所需的資訊。</param>
        private void HandleInvalidRequest(AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;

            logService.Create(LogType.DetectDangerousRequest, string.Format("Detect XSRF,Controller:{0},Action:{1}", controllerName, actionName));

            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            filterContext.Result = new HttpNotFoundResult();
        }
    }
}
